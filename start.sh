#!/usr/bin/env bash
# Start Vehicle Portal: MySQL + Nginx (Docker) and the API (dotnet on host, or Docker if no SDK).
# Frontend: http://localhost:9052  |  API: http://localhost:9051  |  Swagger: http://localhost:9051/swagger
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$ROOT"

if docker compose version >/dev/null 2>&1; then
  dc() { docker compose "$@"; }
elif command -v docker-compose >/dev/null 2>&1; then
  dc() { docker-compose "$@"; }
else
  echo "Error: Install Docker and use 'docker compose' or 'docker-compose'." >&2
  exit 1
fi

wait_for_mysql() {
  echo "Waiting for MySQL to accept connections..."
  local i
  for i in $(seq 1 90); do
    if dc -f db.yml exec -T mysql mysqladmin ping -h localhost -u vehicleuser -pVehicleUser@123 --silent 2>/dev/null; then
      echo "MySQL is ready."
      return 0
    fi
    sleep 1
  done
  echo "Error: MySQL did not become ready in time." >&2
  return 1
}

if command -v dotnet >/dev/null 2>&1; then
  echo "Starting MySQL and Nginx (db.yml)..."
  dc -f db.yml up -d mysql nginx
  wait_for_mysql

  echo ""
  echo "Starting API on http://0.0.0.0:9051 (Ctrl+C stops the API; Docker services keep running)"
  echo "  Portal:  http://localhost:9052"
  echo "  Swagger: http://localhost:9051/swagger"
  echo ""

  cd "$ROOT/VehiclePortal"
  exec dotnet run --launch-profile http
fi

echo ".NET SDK not found on PATH — starting API with Docker (first run may take a few minutes to build)."
echo "To run the API on the host instead, install .NET 9 SDK: https://dotnet.microsoft.com/download"
echo ""
echo "Starting MySQL, Nginx, and API (db.yml)..."
dc -f db.yml up -d --build mysql nginx api
wait_for_mysql

echo ""
echo "All services are up."
echo "  Portal:  http://localhost:9052"
echo "  Swagger: http://localhost:9051/swagger"
echo ""
echo "API container: vehicleportal_api (logs: docker compose -f db.yml logs -f api)"
echo "Stop API only: docker compose -f db.yml stop api"
echo ""

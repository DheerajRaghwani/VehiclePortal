#!/usr/bin/env bash
# Start Vehicle Portal: MySQL + Nginx (Docker) and the .NET API on the host.
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

if ! command -v dotnet >/dev/null 2>&1; then
  echo "Error: .NET SDK not found. Install from https://dotnet.microsoft.com/download" >&2
  exit 1
fi

echo "Starting MySQL and Nginx (db.yml)..."
dc -f db.yml up -d mysql nginx

echo "Waiting for MySQL to accept connections..."
for _ in $(seq 1 90); do
  if dc -f db.yml exec -T mysql mysqladmin ping -h localhost -u vehicleuser -pVehicleUser@123 --silent 2>/dev/null; then
    echo "MySQL is ready."
    break
  fi
  sleep 1
done

echo ""
echo "Starting API on http://0.0.0.0:9051 (Ctrl+C stops the API; Docker services keep running)"
echo "  Portal:  http://localhost:9052"
echo "  Swagger: http://localhost:9051/swagger"
echo ""

cd "$ROOT/VehiclePortal"
exec dotnet run --launch-profile http

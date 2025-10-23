# Vehicle Portal - Port Configuration

## Default Services (Always Running)

| Service         | Port | URL                   | Description                |
| --------------- | ---- | --------------------- | -------------------------- |
| **Frontend**    | 9052 | http://localhost:9052 | Nginx serving static files |
| **Backend API** | 9051 | http://localhost:9051 | .NET Core Web API          |
| **Database**    | 3306 | localhost:3306        | MySQL Database             |

## Optional Services (Run on Demand)

| Service        | Port | URL                   | Command                             | Description               |
| -------------- | ---- | --------------------- | ----------------------------------- | ------------------------- |
| **Adminer**    | 9053 | http://localhost:9053 | `./start-admin-tools.sh adminer`    | Lightweight MySQL admin   |
| **phpMyAdmin** | 9054 | http://localhost:9054 | `./start-admin-tools.sh phpmyadmin` | Full-featured MySQL admin |

## Quick Start Commands

### Start Core Services

```bash
# Start database and frontend
docker-compose -f db.yml up -d mysql nginx

# Start backend API
cd VehiclePortal
dotnet run
```

### Start Admin Tools (Optional)

```bash
# Start both admin tools
./start-admin-tools.sh both

# Start only Adminer
./start-admin-tools.sh adminer

# Start only phpMyAdmin
./start-admin-tools.sh phpmyadmin
```

### Stop Admin Tools

```bash
# Stop Adminer
docker-compose -f db.yml stop adminer

# Stop phpMyAdmin
docker-compose -f db.yml stop phpmyadmin

# Stop both
docker-compose -f db.yml stop adminer phpmyadmin
```

## Database Connection Details

- **Host**: localhost
- **Port**: 3306
- **Database**: vehicle
- **Username**: vehicleuser
- **Password**: VehicleUser@123
- **Root Password**: VehiclePortal@123

## API Endpoints

- **Base URL**: http://localhost:9051
- **Swagger**: http://localhost:9051/swagger
- **Login**: http://localhost:9051/api/Auth/Login

## Frontend Access

- **Main Portal**: http://localhost:9052
- **Login Page**: http://localhost:9052/login
- **Dashboard**: http://localhost:9052/dashboard

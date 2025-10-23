# Vehicle Portal - Database Management

This guide covers the database setup and management for the Vehicle Portal project using MySQL with admin tools.

## 🗄️ **Database Services**

The `db.yml` file provides a complete database management stack:

### **Services Included:**

1. **MySQL 8.0 Database**

   - Port: `3306`
   - Database: `vehicle`
   - User: `vehicleuser`
   - Password: `VehicleUser@123`
   - Root Password: `VehiclePortal@123`

2. **Adminer** (Lightweight Admin Tool)

   - Port: `8081`
   - URL: http://localhost:8081
   - Features: Simple, fast, lightweight

3. **phpMyAdmin** (Full-featured Admin Tool)
   - Port: `8082`
   - URL: http://localhost:8082
   - Features: Complete MySQL administration

## 🚀 **Quick Start**

### **Start Database Services:**

```bash
# Start all database services
./db-manage.sh start

# Or use docker-compose directly
docker-compose -f db.yml up -d
```

### **Access Admin Tools:**

```bash
# Open admin tools in browser
./db-manage.sh admin
```

## 📋 **Database Management Commands**

### **Basic Operations:**

```bash
# Start database services
./db-manage.sh start

# Stop database services
./db-manage.sh stop

# Restart database services
./db-manage.sh restart

# Check status
./db-manage.sh status

# View logs
./db-manage.sh logs
```

### **Backup & Restore:**

```bash
# Create backup
./db-manage.sh backup

# Restore from backup
./db-manage.sh restore backup_20241223_143022.sql

# Reset database (WARNING: Deletes all data)
./db-manage.sh reset
```

### **Admin Tools:**

```bash
# Open admin tools in browser
./db-manage.sh admin
```

## 🔧 **Manual Database Operations**

### **Direct MySQL Access:**

```bash
# Connect to MySQL directly
docker-compose -f db.yml exec mysql mysql -u vehicleuser -pVehicleUser@123 vehicle

# Connect as root
docker-compose -f db.yml exec mysql mysql -u root -pVehiclePortal@123
```

### **Database Backup:**

```bash
# Create backup
docker-compose -f db.yml exec mysql mysqldump -u root -pVehiclePortal@123 vehicle > backup.sql

# Restore backup
docker-compose -f db.yml exec -T mysql mysql -u root -pVehiclePortal@123 vehicle < backup.sql
```

## 🌐 **Access URLs**

After starting the services:

- **MySQL Database**: `localhost:3306`
- **Adminer**: http://localhost:8081
- **phpMyAdmin**: http://localhost:8082

## 🔐 **Default Credentials**

### **MySQL Database:**

- **Host**: `localhost` (or `mysql` from within containers)
- **Port**: `3306`
- **Database**: `vehicle`
- **Username**: `vehicleuser`
- **Password**: `VehicleUser@123`

### **Root Access:**

- **Username**: `root`
- **Password**: `VehiclePortal@123`

## 📊 **Database Schema**

The database includes the following tables:

- `userlogin` - User authentication
- `vehicleregistration` - Vehicle information
- `district` - District data
- `block` - Block/area data
- `checkpost` - Checkpost records
- `checkpostname` - Checkpost names
- `sources` - Source tracking
- `clusternodalregistration` - Nodal registrations
- `nodalregistration` - Nodal officer data

## 🛠️ **Admin Tools Comparison**

### **Adminer (Port 8081)**

- ✅ Lightweight and fast
- ✅ Single file, easy to deploy
- ✅ Modern interface
- ✅ Good for quick queries
- ❌ Limited advanced features

### **phpMyAdmin (Port 8082)**

- ✅ Full-featured MySQL administration
- ✅ Advanced query builder
- ✅ Import/export tools
- ✅ User management
- ✅ Server monitoring
- ❌ Heavier resource usage

## 🔧 **Configuration**

### **Environment Variables:**

Create a `.env` file to customize settings:

```env
# Database Configuration
MYSQL_ROOT_PASSWORD=YourSecurePassword123
MYSQL_DATABASE=vehicle
MYSQL_USER=vehicleuser
MYSQL_PASSWORD=YourUserPassword123
```

### **Custom Configuration:**

Edit `db.yml` to modify:

- Port mappings
- Volume mounts
- Environment variables
- Health check settings

## 🚨 **Troubleshooting**

### **Common Issues:**

1. **Port Already in Use:**

   ```bash
   # Check what's using the port
   sudo netstat -tulpn | grep :3306

   # Kill the process or change ports in db.yml
   ```

2. **Database Connection Failed:**

   ```bash
   # Check MySQL logs
   docker-compose -f db.yml logs mysql

   # Test connection
   docker-compose -f db.yml exec mysql mysqladmin ping -h localhost
   ```

3. **Admin Tools Not Loading:**

   ```bash
   # Check if services are running
   docker-compose -f db.yml ps

   # Restart services
   docker-compose -f db.yml restart
   ```

### **Reset Everything:**

```bash
# Stop and remove all containers, networks, and volumes
docker-compose -f db.yml down -v

# Remove all images
docker system prune -a

# Start fresh
./db-manage.sh start
```

## 📈 **Performance Optimization**

### **MySQL Configuration:**

The database is configured with:

- Connection pooling
- Query caching
- Optimized buffer sizes
- Health monitoring

### **Monitoring:**

```bash
# View resource usage
docker stats

# Check database performance
docker-compose -f db.yml exec mysql mysqladmin status
```

## 🔒 **Security Recommendations**

1. **Change default passwords** in production
2. **Use strong passwords** (32+ characters)
3. **Limit network access** to admin tools
4. **Regular backups** of the database
5. **Monitor access logs** for suspicious activity
6. **Keep Docker images updated**

## 📞 **Support**

If you encounter issues:

1. Check the logs: `./db-manage.sh logs`
2. Verify your configuration in `db.yml`
3. Ensure all required ports are available
4. Check Docker and Docker Compose versions

---

**Happy Database Managing! 🗄️**

-- Create database if not exists
CREATE DATABASE IF NOT EXISTS vehicle CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Use the database
USE vehicle;

-- Create user if not exists
CREATE USER IF NOT EXISTS 'vehicleuser'@'%' IDENTIFIED BY 'VehicleUser@123';

-- Grant privileges
GRANT ALL PRIVILEGES ON vehicle.* TO 'vehicleuser'@'%';

-- Flush privileges
FLUSH PRIVILEGES;

-- Set timezone
SET time_zone = '+00:00';

-- Configure MySQL for better performance
-- Note: Some settings may not be available in all MySQL versions
-- SET GLOBAL innodb_buffer_pool_size = 128M;
SET GLOBAL max_connections = 200;
SET GLOBAL wait_timeout = 28800;
SET GLOBAL interactive_timeout = 28800;

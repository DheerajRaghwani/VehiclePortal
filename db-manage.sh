#!/bin/bash

# Database Management Script for Vehicle Portal
# This script helps manage MySQL database and admin tools

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

# Function to show help
show_help() {
    echo "Database Management Script for Vehicle Portal"
    echo ""
    echo "Usage: $0 [COMMAND]"
    echo ""
    echo "Commands:"
    echo "  start       Start database and admin tools"
    echo "  stop        Stop database and admin tools"
    echo "  restart     Restart database and admin tools"
    echo "  status      Show status of database services"
    echo "  logs        Show logs from database services"
    echo "  backup      Create database backup"
    echo "  restore     Restore database from backup"
    echo "  reset       Reset database (WARNING: This will delete all data)"
    echo "  admin       Open database admin tools in browser"
    echo "  help        Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0 start"
    echo "  $0 backup"
    echo "  $0 admin"
}

# Function to start database services
start_db() {
    print_status "Starting database services..."
    docker-compose -f db.yml up -d
    print_status "Database services started!"
    print_info "Access URLs:"
    print_info "  MySQL Database: localhost:3306"
    print_info "  Adminer: http://localhost:8081"
    print_info "  phpMyAdmin: http://localhost:8082"
}

# Function to stop database services
stop_db() {
    print_status "Stopping database services..."
    docker-compose -f db.yml down
    print_status "Database services stopped!"
}

# Function to restart database services
restart_db() {
    print_status "Restarting database services..."
    docker-compose -f db.yml restart
    print_status "Database services restarted!"
}

# Function to show status
show_status() {
    print_status "Database services status:"
    docker-compose -f db.yml ps
}

# Function to show logs
show_logs() {
    print_status "Showing database logs (Press Ctrl+C to exit):"
    docker-compose -f db.yml logs -f
}

# Function to create backup
backup_db() {
    local backup_file="backup_$(date +%Y%m%d_%H%M%S).sql"
    print_status "Creating database backup: $backup_file"
    docker-compose -f db.yml exec mysql mysqldump -u root -pVehiclePortal@123 vehicle > "$backup_file"
    print_status "Backup created: $backup_file"
}

# Function to restore database
restore_db() {
    if [ -z "$1" ]; then
        print_error "Please provide backup file: $0 restore backup_file.sql"
        exit 1
    fi
    
    if [ ! -f "$1" ]; then
        print_error "Backup file not found: $1"
        exit 1
    fi
    
    print_warning "This will replace all data in the database!"
    read -p "Are you sure? (y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        print_status "Restoring database from: $1"
        docker-compose -f db.yml exec -T mysql mysql -u root -pVehiclePortal@123 vehicle < "$1"
        print_status "Database restored successfully!"
    else
        print_info "Restore cancelled."
    fi
}

# Function to reset database
reset_db() {
    print_warning "This will delete ALL data in the database!"
    read -p "Are you sure? Type 'RESET' to confirm: " -r
    if [[ $REPLY == "RESET" ]]; then
        print_status "Resetting database..."
        docker-compose -f db.yml down -v
        docker-compose -f db.yml up -d
        print_status "Database reset complete!"
    else
        print_info "Reset cancelled."
    fi
}

# Function to open admin tools
open_admin() {
    print_status "Opening database admin tools..."
    
    # Check if services are running
    if ! docker-compose -f db.yml ps | grep -q "Up"; then
        print_warning "Database services are not running. Starting them..."
        start_db
        sleep 10
    fi
    
    print_info "Opening admin tools in your default browser..."
    
    # Try to open in browser (works on macOS and Linux with xdg-open)
    if command -v open >/dev/null 2>&1; then
        # macOS
        open http://localhost:8081 &
        open http://localhost:8082 &
    elif command -v xdg-open >/dev/null 2>&1; then
        # Linux
        xdg-open http://localhost:8081 &
        xdg-open http://localhost:8082 &
    else
        print_info "Please open these URLs in your browser:"
        print_info "  Adminer: http://localhost:8081"
        print_info "  phpMyAdmin: http://localhost:8082"
    fi
}

# Main script logic
case "${1:-help}" in
    start)
        start_db
        ;;
    stop)
        stop_db
        ;;
    restart)
        restart_db
        ;;
    status)
        show_status
        ;;
    logs)
        show_logs
        ;;
    backup)
        backup_db
        ;;
    restore)
        restore_db "$2"
        ;;
    reset)
        reset_db
        ;;
    admin)
        open_admin
        ;;
    help|--help|-h)
        show_help
        ;;
    *)
        print_error "Unknown command: $1"
        echo ""
        show_help
        exit 1
        ;;
esac

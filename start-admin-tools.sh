#!/bin/bash

# Script to start database admin tools on custom ports
# Usage: ./start-admin-tools.sh [adminer|phpmyadmin|both]

case "$1" in
    "adminer")
        echo "Starting Adminer on port 9053..."
        docker-compose -f db.yml up -d adminer
        echo "Adminer is now available at: http://localhost:9053"
        ;;
    "phpmyadmin")
        echo "Starting phpMyAdmin on port 9054..."
        docker-compose -f db.yml up -d phpmyadmin
        echo "phpMyAdmin is now available at: http://localhost:9054"
        ;;
    "both"|"")
        echo "Starting both Adminer and phpMyAdmin..."
        docker-compose -f db.yml up -d adminer phpmyadmin
        echo "Adminer is now available at: http://localhost:9053"
        echo "phpMyAdmin is now available at: http://localhost:9054"
        ;;
    *)
        echo "Usage: $0 [adminer|phpmyadmin|both]"
        echo "  adminer    - Start only Adminer on port 9053"
        echo "  phpmyadmin - Start only phpMyAdmin on port 9054"
        echo "  both       - Start both tools (default)"
        exit 1
        ;;
esac

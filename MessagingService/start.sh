#!/bin/bash

echo "Starting Kafka infrastructure..."
docker-compose -f docker/docker-compose.yml up -d

echo "Waiting for services to be ready..."
sleep 30

echo "Starting application..."
npm start
# EventFlex Kubernetes Deployment Guide

This guide explains how to deploy the EventFlex microservices to a local Kubernetes cluster using Docker Desktop.

## Prerequisites

- Docker Desktop with Kubernetes enabled
- kubectl command-line tool

## Deployment Steps

### 1. Build Docker Images

```bash
# Build the frontend image
cd eventflex-web
docker build -t eventflex-web:latest .
cd ..

# Build the messaging service image
cd MessagingService
docker build -t messaging-service:latest .
cd ..

# Build the notification service image
cd NotificationService
docker build -t notification-service:latest .
cd ..

# Build the user management API image
cd UserManagement.API/UserManagement.API
docker build -t user-management-api:latest -f Dockerfile.k8s .
cd ../..

# Build the event type management API image
cd EventTypeManagement.API
docker build -t event-type-management-api:latest -f Dockerfile .
cd ..
```

### 2. Prepare Secrets

Update the secrets with your real values:

```bash
# Update SMTP credentials in k8s/notification-service/secret.yaml
# Replace BASE64_ENCODED_USERNAME and BASE64_ENCODED_PASSWORD with your actual base64 encoded values

# Update MongoDB credentials in k8s/event-type-management-api/secret.yaml 
# Replace with your actual MongoDB connection string (base64 encoded)
```

### 3. Deploy Infrastructure

```bash
# Deploy Postgres
kubectl apply -f k8s/postgres/

# Deploy MongoDB
kubectl apply -f k8s/mongodb/

# Deploy Kafka and Zookeeper
kubectl apply -f k8s/kafka/
```

### 4. Deploy Microservices

```bash
# Deploy User Management API
kubectl apply -f k8s/user-management-api/

# Deploy Event Type Management API
kubectl apply -f k8s/event-type-management-api/

# Deploy Messaging Service
kubectl apply -f k8s/messaging-service/

# Deploy Notification Service
kubectl apply -f k8s/notification-service/

# Deploy Frontend
kubectl apply -f k8s/eventflex-web/
```

### 5. Configure Local DNS

Add the following entries to your hosts file:

```
127.0.0.1 eventflex.local
127.0.0.1 api.eventflex.local
```

On Windows, the hosts file is located at `C:\Windows\System32\drivers\etc\hosts`.

### 6. Accessing the Application

Once deployed, you can access the application at:

- Frontend: http://eventflex.local
- API: http://api.eventflex.local
- Event Types API: http://api.eventflex.local/event-types

## Troubleshooting

Check the status of your pods:

```bash
kubectl get pods
```

View logs for a specific pod:

```bash
kubectl logs <pod-name>
```

## Cleanup

To remove all deployed resources:

```bash
kubectl delete -f k8s/eventflex-web/
kubectl delete -f k8s/user-management-api/
kubectl delete -f k8s/event-type-management-api/
kubectl delete -f k8s/messaging-service/
kubectl delete -f k8s/notification-service/
kubectl delete -f k8s/kafka/
kubectl delete -f k8s/postgres/
kubectl delete -f k8s/mongodb/
``` 
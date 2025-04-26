# Kafka Messaging Service

This project is a Kafka messaging service built with Node.js and TypeScript. It serves as a core service for communication between various Line of Business (LOB) microservices.

## Table of Contents

- [MessagingFlow](#MessagingFlow)
- [KafkaCommand](#KafkaCommmand)

## MessagingFlow
UserManagement.API → When a user is created, it calls SendMessageAsync("message/user-created", message)
MessagingService → Receives this HTTP request and publishes the message to Kafka on the USER_CREATED topic
NotificationService → Subscribes to the Kafka USER_CREATED topic and sends an email when a message is received

## 自动执行上面的docker compose start.sh: just for development                                 
open git bash
cd /d/Study/MicroService/EventFlex/MessagingService
chmod +x start.sh
./start.sh

## KafkaCommmand
# run Confluence Kafka command to read the events of a topic
docker exec -it kafka /bin/bash
cd /usr/bin
kafka-console-consumer --topic test-topic --from-beginning --bootstrap-server localhost:9092
# events of a consumer group
kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group test-group
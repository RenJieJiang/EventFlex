# README.md

# Notification Service

This project is a Notification Service microservice that sends notifications via email and alerts by consuming messages from a Kafka event queue. It is designed to be containerized and deployed to AWS EC2 or Lambda Function using GitHub Actions.

## Features

- Sends email notifications using SMTP.
- Consumes messages from a Kafka event queue.
- Supports alerts based on consumed messages.
- Containerized for easy deployment.

## Project Structure

```
notification-service
├── src
│   ├── config
│   │   ├── kafka.ts
│   │   └── email.ts
│   ├── services
│   │   ├── notificationService.ts
│   │   └── emailService.ts
│   ├── consumers
│   │   └── messageConsumer.ts
│   ├── types
│   │   └── index.ts
│   └── app.ts
├── .github
│   └── workflows
│       └── deploy.yml
├── Dockerfile
├── package.json
├── tsconfig.json
└── README.md
```

## Setup Instructions

1. Clone the repository:
   ```
   git clone <repository-url>
   cd notification-service
   ```

2. Install dependencies:
   ```
   npm install
   ```

3. Configure the Kafka and email settings in `src/config/kafka.ts` and `src/config/email.ts`.

4. Build the Docker image:
   ```
   docker build -t notification-service .
   ```

5. Run the service:
   ```
   docker run -p 3000:3000 notification-service
   ```

## Usage

- The service will start consuming messages from the Kafka queue and send notifications as configured.
- Ensure that the Kafka broker is running and accessible.

## Deployment

The project includes a GitHub Actions workflow for deploying the service to AWS EC2 or Lambda. Modify the `.github/workflows/deploy.yml` file as needed for your deployment strategy.

## License

This project is licensed under the MIT License.    
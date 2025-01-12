# Kafka Messaging Service

This project is a Kafka messaging service built with Node.js and TypeScript. It serves as a core service for communication between various Line of Business (LOB) microservices.

## Table of Contents

- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Testing](#testing)
- [Docker](#docker)
- [Contributing](#contributing)
- [License](#license)

## Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd kafka-messaging-service
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Create a `.env` file based on the `.env.example` file and configure your environment variables.

## Configuration

The configuration for Kafka is located in `src/config/kafka.config.ts`. You can set the Kafka broker address and authentication details here.

## Usage

To start the messaging service, run the following command:
```bash
npm start
```

This will initialize the Kafka producer and consumer, allowing the service to send and receive messages.

## Testing

Integration tests are located in the `tests/integration` directory. You can run the tests using:
```bash
npm test
```

## Docker

A Docker configuration is provided in the `docker/docker-compose.yml` file. To run Kafka and Zookeeper in Docker, use:
```bash
docker-compose up
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

## 自动执行上面的docker compose start.sh: just for development                         
open git bash
cd /d/Study/MicroService/EventFlex/MessagingService
chmod +x start.sh
./start.sh

## run Confluence Kafka command to read the events of a topic
docker exec -it kafka /bin/bash
cd /usr/bin
kafka-console-consumer --topic test-topic --from-beginning --bootstrap-server localhost:9092
## events of a consumer group
kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group test-group
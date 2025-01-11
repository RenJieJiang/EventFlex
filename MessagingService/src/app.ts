import express, { NextFunction, Request, Response } from 'express';
// import { Kafka } from 'kafkajs';
import kafka from './config/kafka.config';
import KafkaProducer from './services/kafka/producer';
import KafkaConsumer from './services/kafka/consumer';
import config from './config/env.config';
import MessageController from './controllers/messageController';
import { userManagementTopics, eventTypeTopics } from './config/topics.config';

const app = express();
app.use(express.json());

// const kafka = new Kafka({
//     clientId: config.kafka.clientId,
//     brokers: config.kafka.brokers
// });

const producer = new KafkaProducer(kafka);
const consumer = new KafkaConsumer(kafka, config.kafka.groupId);
const messageController = new MessageController(producer);

app.post('/message/user-created', messageController.sendUserCreatedMessage);
app.post('/message/user-updated', messageController.sendUserUpdatedMessage);
app.post('/message/user-deleted', messageController.sendUserDeletedMessage);
app.post('/message/event-type-created', messageController.sendEventCreatedMessage);
app.post('/message/event-type-updated', messageController.sendEventUpdatedMessage);
app.post('/message/event-type-deleted', messageController.sendEventDeletedMessage);

// Health check endpoint
app.get('/health', (req: Request, res: Response) => {
  res.status(200).json({ status: 'UP' });
});

// Global error handling middleware
app.use((err: Error, req: Request, res: Response, next: NextFunction) => {
  console.error(err.stack);
  res.status(500).json({ error: 'Something went wrong!' });
});

const start = async () => {
    try {
      console.log('Kafka Client ID:', process.env.KAFKA_CLIENT_ID);
      console.log('Kafka Brokers:', process.env.KAFKA_BROKERS);
      console.log('Kafka Group ID:', process.env.KAFKA_GROUP_ID );
      console.log('Port:', process.env.PORT);

      await producer.connect();
      // await consumer.connect();

      // const topics = [
      //   ...Object.values(userManagementTopics),
      //   ...Object.values(eventTypeTopics)
      // ];

      // for (const topic of topics) {
      //   await consumer.subscribe(topic);
      // }

      // await consumer.consume(async (message) => {
      //   console.log('Received message:', message);
      // });

      app.listen(config.port, () => {
        console.log(`Server running on port ${config.port}`);
      });
    } catch (error) {
      console.error('Failed to start server:', error);
      process.exit(1);
    }
};

start();
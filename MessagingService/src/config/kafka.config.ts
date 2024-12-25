import { Kafka } from 'kafkajs';
import dotenv from 'dotenv';

dotenv.config();

const kafkaConfig = {
  clientId: process.env.KAFKA_CLIENT_ID || 'my-app',
  brokers: [process.env.KAFKA_BROKER || 'localhost:9092'],
  sasl: {
    mechanism: process.env.KAFKA_MECHANISM as any,
    username: process.env.KAFKA_USERNAME,
    password: process.env.KAFKA_PASSWORD,
  },
  ssl: process.env.KAFKA_SSL === 'true',
};

const kafka = new Kafka(kafkaConfig);

export default kafka;
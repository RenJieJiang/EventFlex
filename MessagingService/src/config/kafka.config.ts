import { Kafka, KafkaConfig } from 'kafkajs';
import dotenv from 'dotenv';
import config from './env.config';

const kafkaConfig: KafkaConfig = {
  clientId: config.kafka.clientId,
  brokers: config.kafka.brokers,
  ssl: process.env.KAFKA_SSL === 'true',
};

if (process.env.KAFKA_MECHANISM && process.env.KAFKA_USERNAME && process.env.KAFKA_PASSWORD) {
  kafkaConfig.sasl = {
    mechanism: process.env.KAFKA_MECHANISM as any,
    username: process.env.KAFKA_USERNAME,
    password: process.env.KAFKA_PASSWORD,
  };
}

const kafka = new Kafka(kafkaConfig);

export default kafka;
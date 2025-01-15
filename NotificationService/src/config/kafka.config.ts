import { Kafka, KafkaConfig } from 'kafkajs';
import dotenv from 'dotenv';
import config from './env.config';

const kafkaConfig: KafkaConfig = {
  clientId: config.kafka.clientId,
  brokers: ['172.31.12.117:9092'], // Hard-coded Kafka broker IP address instead of config.kafka.brokers,
  ssl: process.env.KAFKA_SSL === 'true',
};

if (process.env.KAFKA_MECHANISM && process.env.KAFKA_USERNAME && process.env.KAFKA_PASSWORD) {
  kafkaConfig.sasl = {
    mechanism: process.env.KAFKA_MECHANISM as any,
    username: process.env.KAFKA_USERNAME,
    password: process.env.KAFKA_PASSWORD,
  };
}

console.log('Kafka Configuration:', kafkaConfig);

const kafka = new Kafka(kafkaConfig);

export default kafka;
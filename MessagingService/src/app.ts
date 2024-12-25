import express from 'express';
import { Kafka } from 'kafkajs';
import KafkaProducer from './services/kafka/producer';
import KafkaConsumer from './services/kafka/consumer';
import config from './config/env.config';

const app = express();
app.use(express.json());

const kafka = new Kafka({
    clientId: config.kafka.clientId,
    brokers: config.kafka.brokers
});

const producer = new KafkaProducer(kafka);
const consumer = new KafkaConsumer(kafka, config.kafka.groupId);

app.post('/message', async (req, res) => {
    try {
        await producer.sendMessage('test-topic', req.body);
        res.json({ success: true });
    } catch (error) {
        res.status(500).json({ error: 'Failed to send message' });
    }
});

const start = async () => {
    try {
        await producer.connect();
        await consumer.connect();
        await consumer.subscribe('test-topic');
        await consumer.consume(async (message) => {
            console.log('Received message:', message);
        });

        app.listen(config.port, () => {
            console.log(`Server running on port ${config.port}`);
        });
    } catch (error) {
        console.error('Failed to start server:', error);
        process.exit(1);
    }
};

start();
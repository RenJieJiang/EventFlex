import { Kafka } from 'kafkajs';
import kafka from '../../src/config/kafka.config';
import { v4 as uuidv4 } from 'uuid';
import KafkaProducer from '../../src/services/kafka/producer';
import KafkaConsumer from '../../src/services/kafka/consumer';
import { BaseMessage } from '../../src/interfaces/message.interface';
import { userManagementTopics } from '../../src/config/topics.config';

describe('Kafka Integration Tests', () => {
    const testGroupId = 'test-group';
    const producer = new KafkaProducer(kafka);
    const consumer = new KafkaConsumer(kafka, testGroupId);
    const testTopic = userManagementTopics.USER_CREATED;

    beforeAll(async () => {
        await producer.connect();
        await consumer.connect();
        await consumer.subscribe(testTopic);
    });

    afterAll(async () => {
        await producer?.disconnect();
        await consumer?.disconnect();
    });

    beforeEach(async () => {
      // Clean the topic before each test
      //await consumer.consume(async () => {});
    });

    it('should send and receive messages', async () => {
        const testMessage: BaseMessage = {
            id: uuidv4(),
            timestamp: new Date().toISOString(),
            type: 'USER_CREATED',
            payload: {
                userId: uuidv4(),
                name: 'Test User'
            }
        };

        // Set up message receiver
        const receivedMessages: BaseMessage[] = [];
        await consumer.consume(async (message) => {
            receivedMessages.push(message);
        });

        // Send test message
        await producer.sendMessage(testTopic, testMessage);

        // Wait for message to be received
        await new Promise(resolve => setTimeout(resolve, 1000));

        expect(receivedMessages).toHaveLength(1);
        expect(receivedMessages[0].id).toBe(testMessage.id);
        expect(receivedMessages[0].type).toBe(testMessage.type);
    });
});
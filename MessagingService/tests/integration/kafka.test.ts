import { KafkaProducer } from '../../src/services/kafka/producer';
import { KafkaConsumer } from '../../src/services/kafka/consumer';
import { handleMessage } from '../../src/services/messaging/messageHandler';
import { Message } from '../../src/models/message.model';

describe('Kafka Messaging Service Integration Tests', () => {
    let producer: KafkaProducer;
    let consumer: KafkaConsumer;

    beforeAll(async () => {
        producer = new KafkaProducer();
        consumer = new KafkaConsumer(handleMessage);
        await producer.connect();
        await consumer.connect();
    });

    afterAll(async () => {
        await producer.disconnect();
        await consumer.disconnect();
    });

    it('should send and receive a message', async () => {
        const testMessage: Message = {
            id: '1',
            content: 'Hello Kafka',
            timestamp: new Date().toISOString(),
        };

        await producer.sendMessage(testMessage);

        const receivedMessage = await consumer.consumeMessage();
        expect(receivedMessage).toEqual(testMessage);
    });
});
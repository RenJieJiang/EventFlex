import { Kafka, Consumer, EachMessagePayload } from 'kafkajs';

class KafkaConsumer {
    private consumer: Consumer;

    constructor(kafka: Kafka, groupId: string) {
        this.consumer = kafka.consumer({ groupId });
    }

    async connect() {
        try {
            await this.consumer.connect();
            console.log('Kafka consumer connected');
        } catch (error) {
            console.error('Error connecting Kafka consumer:', error);
            throw error;
        }
    }

    async subscribe(topic: string) {
        try {
            await this.consumer.subscribe({ topic, fromBeginning: true });
            console.log(`Subscribed to topic: ${topic}`);
        } catch (error) {
            console.error('Error subscribing to topic:', error);
            throw error;
        }
    }

    async consume(messageHandler: (message: any) => Promise<void>) {
        try {
            await this.consumer.run({
                eachMessage: async ({ message }: EachMessagePayload) => {
                    const value = message.value?.toString();
                    if (value) {
                        await messageHandler(JSON.parse(value));
                    }
                },
            });
        } catch (error) {
            console.error('Error consuming messages:', error);
            throw error;
        }
    }
}

export default KafkaConsumer;
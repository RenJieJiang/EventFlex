import { Kafka, Producer } from 'kafkajs';

class KafkaProducer {
    private producer: Producer;

    constructor(kafka: Kafka) {
        this.producer = kafka.producer();
    }

    async sendMessage(topic: string, message: any) {
        try {
            await this.producer.send({
                topic: topic,
                messages: [
                    { value: JSON.stringify(message) }
                ],
            });
            console.log(`Message sent to topic ${topic}:`, message);
        } catch (error) {
            console.error('Error sending message:', error);
            throw error;
        }
    }

    async connect() {
        try {
            await this.producer.connect();
            console.log('Kafka producer connected');
        } catch (error) {
            console.error('Error connecting Kafka producer:', error);
            throw error;
        }
    }

    async disconnect() {
        try {
            await this.producer.disconnect();
            console.log('Kafka producer disconnected');
        } catch (error) {
            console.error('Error disconnecting Kafka producer:', error);
            throw error;
        }
    }
}

export default KafkaProducer;
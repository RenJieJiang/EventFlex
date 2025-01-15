import { Consumer, Kafka, EachMessagePayload } from 'kafkajs';
import EmailService from '../services/emailService';
import { userManagementTopics } from '../config/topics.config';
import { UserCreatedMessage } from '../types';

class NotificationConsumer {
    private consumer: Consumer;
    private emailService: EmailService;

    constructor(kafka: Kafka, groupId: string, emailService: EmailService) {
        this.consumer = kafka.consumer({ groupId });
        this.emailService = emailService;
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

    async consume() {
        try {
            await this.consumer.run({
                eachMessage: async ({ message }: EachMessagePayload) => {
                    const value = message.value?.toString();
                    if (value) {
                        const parsedMessage = JSON.parse(value);
                        await this.handleMessage(parsedMessage);
                    }
                },
            });
        } catch (error) {
            console.error('Error consuming messages:', error);
            throw error;
        }
    }

    private async handleMessage(message: any) {
        // Implement your message handling logic here
        console.log('Received message:', message);

        // Example: Send an email notification
        if (message.Type === "USER_CREATED")  {
          const userCreatedMessage = message as UserCreatedMessage;
          console.log('Converted to UserCreatedMessage:', message);
          await this.emailService.send(userCreatedMessage.Email, 'Welcome!', `Hello ${userCreatedMessage.UserName}, welcome to our service!`);
        }
    }

    async disconnect() {
        try {
            await this.consumer.disconnect();
            console.log('Kafka consumer disconnected');
        } catch (error) {
            console.error('Error disconnecting Kafka consumer:', error);
            throw error;
        }
    }
}

export default NotificationConsumer;
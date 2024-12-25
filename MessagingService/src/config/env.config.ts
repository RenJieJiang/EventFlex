import dotenv from 'dotenv';

dotenv.config();

const config = {
    kafka: {
        clientId: process.env.KAFKA_CLIENT_ID || 'messaging-service',
        brokers: (process.env.KAFKA_BROKERS || 'localhost:9092').split(','),
        groupId: process.env.KAFKA_GROUP_ID || 'message-group',
    },
    port: process.env.PORT || 3000
};

export default config;
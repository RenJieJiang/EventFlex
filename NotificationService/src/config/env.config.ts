import dotenv from 'dotenv';
const path = require('path');

// Determine the appropriate .env file based on NODE_ENV
const envFile = process.env.NODE_ENV === 'development' ? '.env.development' : '.env';
dotenv.config({ path: path.resolve(__dirname, '../../', envFile) });

console.log(`Using ${envFile} file`);

const config = {
    kafka: {
        clientId: process.env.KAFKA_CLIENT_ID || 'messaging-service',
        brokers: (process.env.KAFKA_BROKERS || 'kafka:9092').split(','),
        groupId: process.env.KAFKA_GROUP_ID || 'message-group',
    },
    port: process.env.PORT || 3000
};

console.log('Loaded Configuration:', config);

export default config;
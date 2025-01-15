import NotificationConsumer from './consumers/notificationConsumer';
import kafka from './config/kafka.config';
import config from './config/env.config';
import { userManagementTopics, eventTypeTopics } from './config/topics.config';
import EmailService from './services/emailService';
import { emailConfig } from './config/email';

const emailService = new EmailService(emailConfig.smtp);
const consumer = new NotificationConsumer(kafka, config.kafka.groupId, emailService);

const start = async () => {
  await consumer.connect();
  await consumer.subscribe(userManagementTopics.USER_CREATED);
  await consumer.subscribe(eventTypeTopics.EVENT_TYPE_CREATED);
  await consumer.consume();
}

const init = async () => {
  try {
    await start();
    console.log('Notification service started successfully');
  } catch (error) {
    console.error('Error initializing the application:', error);
    process.exit(1);
  }
};

init();
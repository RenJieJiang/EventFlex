

import EmailService from './emailService';

class NotificationService {
    constructor(private emailService: EmailService) {}

    async sendEmail(to: string, subject: string, body: string): Promise<void> {
        await this.emailService.send(to, subject, body);
    }

    async sendAlert(message: string): Promise<void> {
        // Logic to trigger alert based on the consumed message
        console.log(`Alert: ${message}`);
    }
}

export default NotificationService;
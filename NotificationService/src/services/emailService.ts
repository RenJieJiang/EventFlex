import nodemailer from 'nodemailer';

class EmailService {
    private transporter: nodemailer.Transporter;

    constructor(private smtpConfig: any) {
      console.log('SMTP Config:', smtpConfig);
      this.transporter = nodemailer.createTransport(smtpConfig);
    }

    async send(to: string, subject: string, body: string): Promise<void> {
      const mailOptions = {
        from: this.smtpConfig.auth.user,
        to,
        subject,
        text: body,
      };

      try {
        await this.transporter.sendMail(mailOptions);
        console.log(`Email sent to: ${to}`);
      } catch (error) {
        console.error(`Error sending email to: ${to}`, error);
        throw error;
      }
    }
}

export default EmailService;
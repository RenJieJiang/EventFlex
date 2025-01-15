// src/config/email.ts

export const emailConfig = {
    smtp: {
        host: process.env.SMTP_HOST,
        port: parseInt(process.env.SMTP_PORT || '465', 10),
        secure: true, // Use SSL
        auth: {
          user: process.env.SMTP_USER,
          pass: process.env.SMTP_PASS,
        },
    },
    templates: {
        welcome: 'Welcome to our service, {{name}}!',
        alert: 'Alert: {{message}}',
    },
};
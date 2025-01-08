import { Request, Response } from 'express';
import KafkaProducer from '../services/kafka/producer';
import { userManagementTopics, eventTypeTopics } from '../config/topics.config';

class MessageController {
    private producer: KafkaProducer;

    constructor(producer: KafkaProducer) {
        this.producer = producer;
    }

    public sendUserCreatedMessage = async (req: Request, res: Response): Promise<void> => {
        await this.sendMessage(req, res, userManagementTopics.USER_CREATED);
    };

    public sendUserUpdatedMessage = async (req: Request, res: Response): Promise<void> => {
        await this.sendMessage(req, res, userManagementTopics.USER_UPDATED);
    };

    public sendUserDeletedMessage = async (req: Request, res: Response): Promise<void> => {
        await this.sendMessage(req, res, userManagementTopics.USER_DELETED);
    };

    public sendEventCreatedMessage = async (req: Request, res: Response): Promise<void> => {
        await this.sendMessage(req, res, eventTypeTopics.EVENT_CREATED);
    };

    public sendEventUpdatedMessage = async (req: Request, res: Response): Promise<void> => {
        await this.sendMessage(req, res, eventTypeTopics.EVENT_UPDATED);
    }

    public sendEventDeletedMessage = async (req: Request, res: Response): Promise<void> => {
        await this.sendMessage(req, res, eventTypeTopics.EVENT_DELETED);
    }

    private sendMessage = async (req: Request, res: Response, topic: string): Promise<void> => {
        try {
          // Validate request
          if (!req.body || Object.keys(req.body).length === 0) {
            res.status(400).json({ error: 'Invalid request body' });
            return;
          }

          await this.producer.sendMessage(topic, req.body);
          res.json({ success: true });
        } catch (error) {
          res.status(500).json({ error: 'Failed to send message' });
        }
    };
}

export default MessageController;
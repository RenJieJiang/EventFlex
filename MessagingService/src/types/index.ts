export interface Message {
    id: string;
    content: string;
    timestamp: Date;
}

export interface KafkaConfig {
    broker: string;
    username?: string;
    password?: string;
}

export interface EnvConfig {
    KAFKA_BROKER: string;
    KAFKA_USERNAME?: string;
    KAFKA_PASSWORD?: string;
}
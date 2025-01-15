export interface Notification {
    id: string;
    message: string;
    timestamp: Date;
}

export interface Email {
    to: string;
    subject: string;
    body: string;
}

export interface UserCreatedMessage {
  Id: string;
  UserName: string;
  Email: string;
  PhoneNumber: string;
  CreatedAt: Date;
  Type: string; 
}
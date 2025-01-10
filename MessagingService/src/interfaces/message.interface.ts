export interface BaseMessage {
  id: string;
  timestamp: string;
  type: string;
  payload: {
    [key: string]: any;
  };
}

export interface UserMessage extends BaseMessage {
  payload: {
      userId: string;
      [key: string]: any;
  };
}

export interface EventMessage extends BaseMessage {
  payload: {
      eventId: string;
      [key: string]: any;
  };
}
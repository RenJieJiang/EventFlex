export function processMessage(message: any): any {
    // Validate the message structure
    if (!message || !message.id || !message.content) {
        throw new Error("Invalid message format");
    }

    // Transform the message if necessary
    const processedMessage = {
        id: message.id,
        content: message.content.trim(),
        timestamp: new Date().toISOString(),
    };

    // Additional processing logic can be added here

    return processedMessage;
}
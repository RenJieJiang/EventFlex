namespace EventTypeManagement.API.Messages
{
    public class EventTypeCreatedMessage
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

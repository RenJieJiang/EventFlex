using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EventTypeManagement.API.Models
{
    public class EventType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [StringLength(100)]
        [BsonElement("name")]
        public required string Name { get; set; }

        [StringLength(500)]
        [BsonElement("description")]
        public string? Description { get; set; }
    }
}

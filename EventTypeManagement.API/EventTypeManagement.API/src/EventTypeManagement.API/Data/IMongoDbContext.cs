using EventTypeManagement.API.Models;
using MongoDB.Driver;

namespace EventTypeManagement.API.Data
{
    public interface IMongoDbContext
    {
        string ConnectionString { get; }
        IMongoDatabase Database { get; }
        IMongoCollection<EventType> EventTypes { get; }
    }
}

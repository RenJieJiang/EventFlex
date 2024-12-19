using EventTypeManagement.API.Models;
using MongoDB.Driver;

namespace EventTypeManagement.API.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("EventTypeDb");
        }

        public IMongoCollection<EventType> EventTypes => _database.GetCollection<EventType>("EventTypes");
    }
}

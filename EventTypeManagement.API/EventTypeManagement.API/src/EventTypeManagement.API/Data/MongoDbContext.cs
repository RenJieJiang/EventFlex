using EventTypeManagement.API.Models;
using MongoDB.Driver;

namespace EventTypeManagement.API.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public string ConnectionString { get; }
        public IMongoDatabase Database { get { return _database; }  }

        public MongoDbContext(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("MongoDb") ?? "";
            var client = new MongoClient(ConnectionString);
            _database = client.GetDatabase("event_type_db");

            CreateIndexes();
        }

        public IMongoCollection<EventType> EventTypes => _database.GetCollection<EventType>("event_types");

        private void CreateIndexes()
        {
            var indexKeysDefinition = Builders<EventType>.IndexKeys.Ascending(et => et.Name);
            var indexModel = new CreateIndexModel<EventType>(indexKeysDefinition);
            EventTypes.Indexes.CreateOne(indexModel);
        }
    }
}

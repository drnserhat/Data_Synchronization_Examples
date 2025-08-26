using MongoDB.Driver;

namespace ServiceB.Services
{
    
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        public MongoDBService(IConfiguration configuration)
        {
            MongoClient client = new(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase(configuration["DatabaseSettings:ServiceBDB"]);
        }

        public IMongoCollection<T> GetCollection<T>() => _database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }
}

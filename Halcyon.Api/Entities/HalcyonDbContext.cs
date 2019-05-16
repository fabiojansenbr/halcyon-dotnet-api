using Halcyon.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Halcyon.Api.Entities
{
    public class HalcyonDbContext
    {
        private readonly IMongoDatabase _database = null;

        public HalcyonDbContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("camelCase", pack, t => true);

            var settings = mongoDBSettings.Value;
            var mongoUrl = new MongoUrl(settings.Uri);
            var databaseName = mongoUrl.DatabaseName;

            var client = new MongoClient(mongoUrl);
            if (client != null)
            {
                _database = client.GetDatabase(databaseName);
            }
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }
}
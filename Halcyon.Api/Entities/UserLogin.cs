using Halcyon.Api.Services.Providers;
using MongoDB.Bson.Serialization.Attributes;

namespace Halcyon.Api.Entities
{
    public class UserLogin
    {
        public UserLogin(Provider provider, string externalId)
        {
            Provider = provider;
            ExternalId = externalId;
        }

        [BsonRequired]
        public Provider Provider { get; set; }

        [BsonRequired]
        public string ExternalId { get; set; }
    }
}
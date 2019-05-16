using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Halcyon.Api.Entities
{
    public class UserRefreshToken
    {
        public UserRefreshToken()
        {
            Token = Guid.NewGuid().ToString();
            Issued = DateTime.UtcNow;
        }

        [BsonRequired]
        public string Token { get; set; }

        [BsonRequired]
        [BsonDateTimeOptions]
        public DateTime Issued { get; set; }
    }
}
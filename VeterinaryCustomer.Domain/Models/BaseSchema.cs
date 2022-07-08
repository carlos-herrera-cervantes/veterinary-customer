using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VeterinaryCustomer.Domain.Models
{
    public class BaseSchema
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("created_at")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}

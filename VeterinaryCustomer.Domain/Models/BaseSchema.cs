using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace VeterinaryCustomer.Domain.Models;

public class BaseSchema
{
    [BsonElement("_id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("id")]
    public string Id { get; set; }

    [BsonElement("created_at")]
    [BsonRepresentation(BsonType.DateTime)]
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    [BsonRepresentation(BsonType.DateTime)]
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

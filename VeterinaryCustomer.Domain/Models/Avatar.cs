using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace VeterinaryCustomer.Domain.Models;

public class Avatar : BaseSchema
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("customer_id")]
    [JsonProperty("customer_id")]
    public string CustomerId { get; set; }

    [BsonElement("path")]
    [JsonProperty("path")]
    public string Path { get; set; }
}

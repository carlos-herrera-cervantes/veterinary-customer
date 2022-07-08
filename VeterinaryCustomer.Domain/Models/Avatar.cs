using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VeterinaryCustomer.Domain.Models
{
    public class Avatar : BaseSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("customer_id")]
        public int CustomerId { get; set; }

        [BsonElement("path")]
        public string Path { get; set; }
    }
}

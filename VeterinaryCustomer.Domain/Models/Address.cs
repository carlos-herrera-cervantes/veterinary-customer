using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace VeterinaryCustomer.Domain.Models
{
    public class Address : BaseSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("customer_id")]
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [BsonElement("city")]
        [JsonProperty("city")]
        public string City { get; set; }

        [BsonElement("postal_code")]
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [BsonElement("street")]
        [JsonProperty("street")]
        public string Street { get; set; }

        [BsonElement("colony")]
        [JsonProperty("colony")]
        public string Colony { get; set; }

        [BsonElement("number")]
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}

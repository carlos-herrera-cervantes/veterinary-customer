using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VeterinaryCustomer.Domain.Models
{
    public class Address : BaseSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("customer_id")]
        public int CustomerId { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("postal_code")]
        public string PostalCode { get; set; }

        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("colony")]
        public string Colony { get; set; }

        [BsonElement("number")]
        public string Number { get; set; }
    }
}

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VeterinaryCustomer.Domain.Models
{
    public class Profile : BaseSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("customer_id")]
        public int CustomerId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("last_name")]
        public string LastName { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement("birthdate")]
        public DateTime Birthdate { get; set; }
    }
}

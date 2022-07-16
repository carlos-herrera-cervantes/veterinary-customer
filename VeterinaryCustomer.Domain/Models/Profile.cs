using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using VeterinaryCustomer.Domain.Enums;

namespace VeterinaryCustomer.Domain.Models
{
    public class Profile : BaseSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("customer_id")]
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("last_name")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [BsonElement("gender")]
        [JsonProperty("gender")]
        public string Gender { get; set; } = Genders.NotSpecified;

        [BsonElement("phone_number")]
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [BsonElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement("birthday")]
        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }
    }

    public class UpdateProfileDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("gender")]
        public string Gender { get; set; }
        
        [JsonProperty("birthday")]
        public DateTime? Birthday { get; set; }
    }
}

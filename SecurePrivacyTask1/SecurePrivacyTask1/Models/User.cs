using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SecurePrivacyTask1.Models
{
    public class User(string userName, string password, string email, string phone, string address, string city)
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string UserName { get; set; } = userName;
        public string Password { get; set; } = password;
        public string Email { get; set; } = email;
        public string Phone { get; set; } = phone;
        public string Address { get; set; } = address;
        public string City { get; set; } = city;
        public bool ConsentGiven { get; set; } // New field for consent

        public override string ToString()
        {
            return $"Id: {Id}, UserName: {UserName}, Email: {Email}, Phone: {Phone}, Address: {Address}, City: {City}, ConsentGiven: {ConsentGiven}";
        }
    }
}

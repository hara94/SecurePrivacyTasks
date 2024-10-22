using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SecurePrivacyTask1.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public bool ConsentGiven { get; set; }

        // permissions added directly to the user for simplicty
        public bool CanCreateUsers { get; set; }
        public bool CanDeleteUsers { get; set; }
        public bool CanEditUsers { get; set; }

        // Tracking Information
        public DateTime CreatedAt { get; set; }  // Creation time
        public string CreatedByUserId { get; set; }  // The user who created this account

        public User()
        {
        }

        public User(string userName, string passwordHash, string email)
        {
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            ConsentGiven = true;
            CanCreateUsers = false;
            CanDeleteUsers = false;
            CanEditUsers = false;
        }

        public User(string userName,
            string passwordHash,
            string email,
            string? phone,
            string? address,
            string? city,
            bool consentGiven,
            bool canCreateUsers,
            bool canDeleteUsers,
            bool canEditUsers
            )
        {
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            Phone = phone;
            Address = address;
            City = city;
            ConsentGiven = consentGiven;
            CanCreateUsers = canCreateUsers;
            CanDeleteUsers = canDeleteUsers;
            CanEditUsers = canEditUsers;
        }

        public override string ToString()
        {
            return $"User: {UserName}";
        }
    }
}

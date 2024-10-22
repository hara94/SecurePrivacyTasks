using MongoDB.Bson;
using MongoDB.Driver;
using SecurePrivacyTask1.Models;

public class MongoDBSeed
{
    private readonly IMongoCollection<User> _usersCollection;

    public MongoDBSeed(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<User>("Users");
    }

    public async Task SeedAsync()
    {
        // Check if the collection is empty before seeding
        var usersCount = await _usersCollection.CountDocumentsAsync(Builders<User>.Filter.Empty);

        if (usersCount == 0)
        {
            // Generate ObjectIds for users
            var user1Id = ObjectId.GenerateNewId();
            var user2Id = ObjectId.GenerateNewId();

            // Seed initial data if collection is empty
            var initialUsers = new List<User>
            {
                // First user
                new User
                {
                    Id = user1Id.ToString(),
                    UserName = "john_doe",
                    PasswordHash = "$2a$11$n6BsGi1.Hs8a6HfIEhxz6.wYE89RDc4P1NinAHIhxr3s7czPwuWrC", // Example hashed password
                    Email = "john@example.com",
                    Phone = "123456789",
                    Address = "123 Main St",
                    City = "New York",
                    ConsentGiven = true,
                    CanCreateUsers = true,
                    CanDeleteUsers = true,
                    CanEditUsers = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = null  // The first user is created by the system
                },

                // Second user created by the first user
                new User
                {
                    Id = user2Id.ToString(),
                    UserName = "jane_smith",
                    PasswordHash = "$2a$11$n6BsGi1.Hs8a6HfIEhxz6.wYE89RDc4P1NinAHIhxr3s7czPwuWrC", // Example hashed password
                    Email = "jane@example.com",
                    Phone = "987654321",
                    Address = "456 Oak St",
                    City = "San Francisco",
                    ConsentGiven = true,
                    CanCreateUsers = true,
                    CanDeleteUsers = true,
                    CanEditUsers = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = user1Id.ToString()  // Created by the first user
                }
            };

            await _usersCollection.InsertManyAsync(initialUsers);
        }

        // Create the composite index on UserName and Email (unique constraint)
        var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.UserName).Ascending(u => u.Email);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<User>(indexKeys, indexOptions);

        await _usersCollection.Indexes.CreateOneAsync(indexModel);
    }
}

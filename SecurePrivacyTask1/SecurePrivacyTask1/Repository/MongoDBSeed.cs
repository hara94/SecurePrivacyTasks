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
            // Seed initial data if collection is empty
            var initialUsers = new List<User>
            {
                new("john_doe", "password123", "john@example.com", "123456789", "123 Main St", "New York"),
                new("jane_smith", "password456", "jane@example.com", "987654321", "456 Oak St", "San Francisco")
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

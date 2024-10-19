using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurePrivacyTask1.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : class
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<TDocument>(collectionName);
        }

        public async Task<IEnumerable<TDocument>> GetAllAsync()
        {
            return await _collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync();
        }

        public async Task<TDocument> GetByIdAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateAsync(string id, TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, document);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<bool> IsUserNameUniqueAsync(string userName)
        {
            var filter = Builders<TDocument>.Filter.Eq("UserName", userName);
            return await _collection.Find(filter).AnyAsync() == false; // Returns true if no match found
        }
    }
}

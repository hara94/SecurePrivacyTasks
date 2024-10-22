using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                throw new ArgumentException("Invalid id format");
            }

            var filter = Builders<TDocument>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateAsync(string id, TDocument document)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                throw new ArgumentException("Invalid id format");
            }

            var filter = Builders<TDocument>.Filter.Eq("_id", objectId);
            await _collection.ReplaceOneAsync(filter, document);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<bool> UserExists(string userName)
        {
            var filter = Builders<TDocument>.Filter.Eq("UserName", userName);
            return await _collection.Find(filter).AnyAsync();
        }

        public async Task<TDocument> GetByUsernameAsync(string userName)
        {
            var filter = Builders<TDocument>.Filter.Eq("UserName", userName);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).ToListAsync();
        }
    }
}

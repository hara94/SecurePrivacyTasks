using System.Linq.Expressions;

namespace SecurePrivacyTask1.Repository
{
    public interface IMongoRepository<TDocument> where TDocument : class
    {
        Task<IEnumerable<TDocument>> GetAllAsync();
        Task<TDocument> GetByIdAsync(string id);
        Task AddAsync(TDocument document);
        Task UpdateAsync(string id, TDocument document);
        Task DeleteAsync(string id);
        Task<bool> UserExists(string username);
        Task<TDocument> GetByUsernameAsync(string userName);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}

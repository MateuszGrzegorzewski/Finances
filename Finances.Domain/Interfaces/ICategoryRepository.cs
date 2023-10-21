using Finances.Domain.Entities;

namespace Finances.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(Category category);

        Task<Category?> GetByName(string name, string currentUserId);

        Task<IEnumerable<Category>> GetAll(string currentUserId);

        Task<Category> GetByEncodedName(string encodedName, string currentUserId);

        Task Delete(Category category);
    }
}
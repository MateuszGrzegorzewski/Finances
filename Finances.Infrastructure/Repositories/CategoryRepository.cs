using Finances.Domain.Entities;
using Finances.Domain.Interfaces;
using Finances.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finances.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ExpenseDbContext _dbContext;

        public CategoryRepository(ExpenseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Category category)
        {
            _dbContext.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _dbContext.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAll(string currentUserId)
            => await _dbContext.Categories.Where(c => c.CreatedById == null || c.CreatedById == currentUserId).ToListAsync();

        public async Task<Category> GetByEncodedName(string encodedName, string currentUserId)
            => await _dbContext.Categories.Where(c => c.CreatedById == null || c.CreatedById == currentUserId).FirstAsync(x => x.EncodedName == encodedName);

        public Task<Category?> GetByName(string name, string currentUserId)
            => _dbContext.Categories.Where(c => c.CreatedById == null || c.CreatedById == currentUserId).FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }
}
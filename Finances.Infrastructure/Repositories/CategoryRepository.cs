using Finances.Domain.Entities;
using Finances.Domain.Interfaces;
using Finances.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Category>> GetAll()
            => await _dbContext.Categories.ToListAsync();

        public Task<Category?> GetByName(string name)
            => _dbContext.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }
}
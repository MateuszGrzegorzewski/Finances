using Finances.Domain.Interfaces;
using Finances.Infrastructure.Persistence;
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

        public async Task Create(Domain.Entities.Category category)
        {
            _dbContext.Add(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
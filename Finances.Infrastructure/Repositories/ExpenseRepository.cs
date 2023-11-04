using Finances.Domain.Entities;
using Finances.Domain.Interfaces;
using Finances.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finances.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseDbContext _dbContext;

        public ExpenseRepository(ExpenseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        => await _dbContext.SaveChangesAsync();

        public async Task Create(Expense expense)
        {
            _dbContext.Add(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Expense expense)
        {
            _dbContext.Remove(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Expense>> GetAll(string currentUserId)
        => await _dbContext.Expenses.Where(c => c.CreatedById == currentUserId).OrderByDescending(e => e.CreatedAt).ThenBy(e => e.Category).ToListAsync();

        public async Task<IEnumerable<Expense>> GetAllByCategory(string category, string currentUserId)
        => await _dbContext.Expenses.Where(c => c.CreatedById == currentUserId).Where(x => x.Category == category).ToListAsync();

        public async Task<Expense> GetById(int id, string currentUserId)
        => await _dbContext.Expenses.Where(c => c.CreatedById == currentUserId).FirstAsync(x => x.Id == id);
    }
}
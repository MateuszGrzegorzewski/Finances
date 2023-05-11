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

        public async Task<IEnumerable<Expense>> GetAll()
        => await _dbContext.Expenses.ToListAsync();

        public async Task<Expense> GetById(int id)
        => await _dbContext.Expenses.FirstAsync(x => x.Id == id);
    }
}
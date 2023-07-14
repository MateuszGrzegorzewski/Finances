using Finances.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finances.Infrastructure.Persistence
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
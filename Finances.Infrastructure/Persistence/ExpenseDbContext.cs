using Microsoft.EntityFrameworkCore;

namespace Finances.Infrastructure.Persistence
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Expense> Expenses { get; set; }

        public DbSet<Domain.Entities.Category> Categories { get; set; }
    }
}
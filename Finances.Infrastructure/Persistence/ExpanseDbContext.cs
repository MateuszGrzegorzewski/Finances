using Microsoft.EntityFrameworkCore;

namespace Finances.Infrastructure.Persistence
{
    public class ExpanseDbContext : DbContext
    {
        public ExpanseDbContext(DbContextOptions<ExpanseDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Expanse> Expanses { get; set; }
    }
}
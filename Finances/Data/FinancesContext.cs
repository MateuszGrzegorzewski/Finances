using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Finances.Models;

namespace Finances.Data
{
    public class FinancesContext : DbContext
    {
        public FinancesContext(DbContextOptions<FinancesContext> options)
            : base(options)
        {
        }

        public DbSet<Finances.Models.Expanse> Expanse { get; set; } = default!;
    }
}
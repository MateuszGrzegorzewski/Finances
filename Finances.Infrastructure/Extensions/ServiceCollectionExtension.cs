using Finances.Domain.Interfaces;
using Finances.Infrastructure.Persistence;
using Finances.Infrastructure.Repositories;
using Finances.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExpenseDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("Expense")));

            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<CategorySeeder>();
        }
    }
}
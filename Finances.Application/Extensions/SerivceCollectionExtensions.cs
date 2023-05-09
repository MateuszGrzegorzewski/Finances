using Finances.Application.Expense;
using Finances.Application.Mappings;
using Finances.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();

            services.AddAutoMapper(typeof(ExpenseMappingProfile));

            services.AddValidatorsFromAssemblyContaining<ExpenseDtoValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }
    }
}
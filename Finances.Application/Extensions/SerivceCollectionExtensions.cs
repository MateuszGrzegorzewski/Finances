using Finances.Application.ApplicationUser;
using Finances.Application.Expense.Commands.CreateCategory;
using Finances.Application.Expense.Commands.CreateExpense;
using Finances.Application.Mappings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Finances.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateExpenseCommand)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateCategoryCommand)));

            services.AddAutoMapper(typeof(ExpenseMappingProfile));
            services.AddAutoMapper(typeof(CategoryMappingProfile));

            services.AddValidatorsFromAssemblyContaining<CreateExpenseCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }
    }
}
using Finances.Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(ICategoryRepository repository)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(2).WithMessage("Name should have at least 2 characters")
                .MaximumLength(20).WithMessage("Name should have maximum 20 characters")
                .Custom((value, context) =>
                {
                    var existingCategory = repository.GetByName(value).Result;
                    if (existingCategory != null)
                    {
                        context.AddFailure($"{value} is not unique name for category");
                    }
                });
        }
    }
}
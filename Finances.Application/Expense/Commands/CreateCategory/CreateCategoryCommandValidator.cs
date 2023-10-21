using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using FluentValidation;

namespace Finances.Application.Expense.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(ICategoryRepository repository, IUserContext userContext)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(2).WithMessage("Name should have at least 2 characters")
                .MaximumLength(20).WithMessage("Name should have maximum 20 characters")
                .Custom((value, context) =>
                {
                    var currentUserId = userContext.GetCurrentUser().Id;
                    var existingCategory = repository.GetByName(value, currentUserId).Result;
                    if (existingCategory != null)
                    {
                        context.AddFailure($"{value} is not unique name for category");
                    }
                });
        }
    }
}
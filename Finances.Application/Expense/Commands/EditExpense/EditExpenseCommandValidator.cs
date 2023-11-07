using FluentValidation;

namespace Finances.Application.Expense.Commands.EditExpense
{
    public class EditExpenseCommandValidator : AbstractValidator<EditExpenseCommand>
    {
        public EditExpenseCommandValidator()
        {
            {
                RuleFor(c => c.Value)
                    .NotEmpty().WithMessage("Please enter value.")
                    .GreaterThanOrEqualTo(0).WithMessage("Value have to be greater or equal 0.")
                    .PrecisionScale(18, 2, false).WithMessage("Incorrect value - maximum 2 decimal places");

                RuleFor(c => c.Category)
                    .NotEmpty().WithMessage("Please choose Category.");

                RuleFor(c => c.Description)
                    .MaximumLength(256).WithMessage("Description should have maximum of 256 characters.");
            }
        }
    }
}
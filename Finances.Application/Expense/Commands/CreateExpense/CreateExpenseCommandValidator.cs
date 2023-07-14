using Finances.Application.Expense.Query.GetAllCategories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseCommandValidator()
        {
            RuleFor(c => c.Value)
                .NotEmpty().WithMessage("Please enter value.")
                .GreaterThanOrEqualTo(0).WithMessage("Value have to be greater or equal 0.")
                .PrecisionScale(18, 2, false).WithMessage("Incorrect value - maximum 2 decimal places");

            RuleFor(c => c.Category)
                .NotEmpty().WithMessage("Please choose Category.");
            //.MinimumLength(2)
            //.MaximumLength(40);

            RuleFor(c => c.Description)
                .MaximumLength(256).WithMessage("Description should have maximum of 256 characters.");
        }
    }
}
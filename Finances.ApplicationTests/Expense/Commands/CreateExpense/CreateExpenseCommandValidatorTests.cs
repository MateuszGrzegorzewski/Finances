using FluentValidation.TestHelper;
using Xunit;

namespace Finances.Application.Expense.Commands.CreateExpense.Tests
{
    public class CreateExpenseCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand()
            {
                Value = 15,
                Category = "Home",
                Description = "Test description",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInValidCommand_ShouldHaveValidationErrors()
        {
            var validator = new CreateExpenseCommandValidator();
            var command = new CreateExpenseCommand()
            {
                Value = -1,
                Category = null,
                Description = "Test description",
                CreatedAt = DateTime.Now.AddDays(1)
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(e => e.Value);
            result.ShouldHaveValidationErrorFor(e => e.Category);
            result.ShouldHaveValidationErrorFor(e => e.CreatedAt);
        }
    }
}
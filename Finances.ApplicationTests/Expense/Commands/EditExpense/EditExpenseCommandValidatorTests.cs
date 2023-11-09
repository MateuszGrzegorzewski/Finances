using FluentValidation.TestHelper;
using Xunit;

namespace Finances.Application.Expense.Commands.EditExpense.Tests
{
    public class EditExpenseCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            var validator = new EditExpenseCommandValidator();
            var command = new EditExpenseCommand()
            {
                Value = 15,
                Category = "Home",
                Description = "Test description",
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInValidCommand_ShouldHaveValidationErrors()
        {
            var validator = new EditExpenseCommandValidator();
            var command = new EditExpenseCommand()
            {
                Value = -1,
                Category = null,
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(e => e.Value);
            result.ShouldHaveValidationErrorFor(e => e.Category);
        }
    }
}
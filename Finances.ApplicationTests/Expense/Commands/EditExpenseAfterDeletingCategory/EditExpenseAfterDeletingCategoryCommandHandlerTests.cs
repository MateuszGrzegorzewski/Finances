using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory.Tests
{
    public class EditExpenseAfterDeletingCategoryCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_EditExpenseAfterDeletingCategory_WithAuthorizedUser()
        {
            var expensesDto = new List<ExpenseDto>
                {
                    new ExpenseDto
                    {
                        Value = 20,
                        Category = "Home",
                    }
                };

            var expensesCommandDto = Task.FromResult<IEnumerable<ExpenseDto>>(expensesDto);

            var command = new EditExpenseAfterDeletingCategoryCommand(expensesCommandDto);

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetById(expensesDto.First().Id, user.Id))
                .ReturnsAsync(new Domain.Entities.Expense() { Value = expensesDto.First().Value });

            var handler = new EditExpenseAfterDeletingCategoryCommandHandler(expenseRepositoryMock.Object, userContextMock.Object);

            await handler.Handle(command, CancellationToken.None);

            expenseRepositoryMock.Verify(m => m.GetById(expensesDto.First().Id, user.Id), Times.Once);
        }
    }
}
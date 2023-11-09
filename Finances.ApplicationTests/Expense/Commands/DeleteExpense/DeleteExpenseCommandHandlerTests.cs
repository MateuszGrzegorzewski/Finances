using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Expense.Commands.DeleteExpense.Tests
{
    public class DeleteExpenseCommandHandlerTests
    {
        [Fact()]
        public async Task DeleteExpenseCommandHandler_DeleteExpense_WithAuthorizedUser()
        {
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var command = new DeleteExpenseCommand();

            var userContetMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContetMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetById(expense.Id, user.Id));

            var handler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, userContetMock.Object);

            await handler.Handle(command, CancellationToken.None);

            expenseRepositoryMock.Verify(m => m.Delete(It.IsAny<Domain.Entities.Expense>()), Times.Once);
        }
    }
}
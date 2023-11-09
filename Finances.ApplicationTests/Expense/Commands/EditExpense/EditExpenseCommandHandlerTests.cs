using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Finances.Application.Expense.Commands.EditExpense.Tests
{
    public class EditExpenseCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_EditExpense_WithAuthorizedUser()
        {
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                Description = "Test description",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var command = new EditExpenseCommand()
            {
                Id = 1,
                Value = 20,
                Category = "Health",
                Description = "Test description after edit",
            };

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var handler = new EditExpenseCommandHandler(expenseRepositoryMock.Object, userContextMock.Object);

            await handler.Handle(command, CancellationToken.None);

            expenseRepositoryMock.Verify(m => m.GetById(command.Id, user.Id), Times.Once);
            expenseRepositoryMock.Verify(m => m.Commit(), Times.Once);
            expense.Value.Should().Be(20);
            expense.Category.Should().Be("Health");
            expense.Description.Should().Be("Test description after edit");
        }
    }
}
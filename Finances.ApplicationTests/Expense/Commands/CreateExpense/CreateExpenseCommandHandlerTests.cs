using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Mappings;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Expense.Commands.CreateExpense.Tests
{
    public class CreateExpenseCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_CreateExpense_WithAuthorizedUser()
        {
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                Description = "Test description",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var command = new CreateExpenseCommand();

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@example.com"));

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.Create(expense));

            var myProfile = new ExpenseMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var handler = new CreateExpenseCommandHandler(expenseRepositoryMock.Object, mapper, userContextMock.Object);

            await handler.Handle(command, CancellationToken.None);

            expenseRepositoryMock.Verify(m => m.Create(It.IsAny<Domain.Entities.Expense>()), Times.Once);
        }
    }
}
using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Mappings;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Expense.Query.GetByIdExpense.Tests
{
    public class GetByIdExpenseQueryHandlerTests
    {
        [Fact()]
        public async Task Handle_GetByIdExpense_WithAuthorizedUser()
        {
            var expense = new Domain.Entities.Expense()
            {
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var query = new GetByIdExpenseQuery(expense.Id);

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var myProfile = new ExpenseMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var handler = new GetByIdExpenseQueryHandler(expenseRepositoryMock.Object, mapper, userContextMock.Object);

            await handler.Handle(query, CancellationToken.None);

            expenseRepositoryMock.Verify(m => m.GetById(query.Id, user.Id), Times.Once);
        }
    }
}
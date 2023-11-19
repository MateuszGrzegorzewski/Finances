using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Mappings;
using Finances.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Finances.Application.Expense.Query.GetAllExpensesByCategory.Tests
{
    public class GetAllExpensesByCategoryQueryHandlerTests
    {
        [Fact()]
        public async Task Handle_GetAllExpensesByCategory_WithAuthorizedUser()
        {
            var expenses = new List<Domain.Entities.Expense>();

            var expense1 = new Domain.Entities.Expense()
            {
                Value = 15,
                Category = "Home",
                CreatedById = "1"
            };

            var expense2 = new Domain.Entities.Expense()
            {
                Value = 20,
                Category = "Home",
                CreatedById = "1"
            };

            expenses.Add(expense1);
            expenses.Add(expense2);

            var category = new Domain.Entities.Category()
            {
                Name = "Home",
            };

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var query = new GetAllExpensesByCategoryQuery(category.Name);

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetAllByCategory(category.Name, user.Id))
                .ReturnsAsync(expenses);

            var myProfile = new ExpenseMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var handler = new GetAllExpensesByCategoryQueryHandler(expenseRepositoryMock.Object, mapper, userContextMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            expenseRepositoryMock.Verify(m => m.GetAllByCategory(category.Name, user.Id), Times.Once);
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
    }
}
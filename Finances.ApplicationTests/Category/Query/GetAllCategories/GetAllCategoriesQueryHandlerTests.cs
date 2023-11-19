using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Mappings;
using Finances.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Finances.Application.Category.Query.GetAllCategories.Tests
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact()]
        public async Task Handle_GetAllCategories_WithAuthorizedUser()
        {
            var categories = new List<Domain.Entities.Category>();

            var homeCategory = new Domain.Entities.Category()
            {
                Name = "Home",
                CreatedById = "1"
            };

            var healthCategory = new Domain.Entities.Category()
            {
                Name = "Health",
                CreatedById = "1"
            };

            categories.Add(homeCategory);
            categories.Add(healthCategory);

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var query = new GetAllCategoriesQuery();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(c => c.GetAll(user.Id))
                .ReturnsAsync(categories);

            var myProfile = new CategoryMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var handler = new GetAllCategoriesQueryHandler(categoryRepositoryMock.Object, mapper, userContextMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            categoryRepositoryMock.Verify(m => m.GetAll(user.Id), Times.Once);
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
    }
}
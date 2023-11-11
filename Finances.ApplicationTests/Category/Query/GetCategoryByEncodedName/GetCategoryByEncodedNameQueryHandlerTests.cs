using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Mappings;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Category.Query.GetCategoryByEncodedName.Tests
{
    public class GetCategoryByEncodedNameQueryHandlerTests
    {
        [Fact()]
        public async Task Handle_GetCategoryByEncodedName_WithAuthorizedUser()
        {
            var category = new Domain.Entities.Category()
            {
                Name = "Home",
                CreatedById = "1",
            };

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var query = new GetCategoryByEncodedNameQuery(category.EncodedName);

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(c => c.GetByEncodedName(category.EncodedName, user.Id))
                .ReturnsAsync(category);

            var myProfile = new CategoryMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var handler = new GetCategoryByEncodedNameQueryHandler(categoryRepositoryMock.Object, mapper, userContextMock.Object);

            await handler.Handle(query, CancellationToken.None);

            categoryRepositoryMock.Verify(m => m.GetByEncodedName(category.EncodedName, user.Id), Times.Once);
        }
    }
}
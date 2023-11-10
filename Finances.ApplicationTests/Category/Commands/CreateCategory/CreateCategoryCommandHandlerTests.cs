using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Mappings;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Category.Commands.CreateCategory.Tests
{
    public class CreateCategoryCommandHandlerTests
    {
        [Fact()]
        public async void Handle_CreateExpense_WithAuthorizedUser()
        {
            var command = new CreateCategoryCommand()
            {
                Name = "Home"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@example.com"));

            var categoyRepositoryMock = new Mock<ICategoryRepository>();

            var myProfile = new CategoryMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var handler = new CreateCategoryCommandHandler(categoyRepositoryMock.Object, mapper, userContextMock.Object);

            await handler.Handle(command, CancellationToken.None);

            categoyRepositoryMock.Verify(m => m.Create(It.IsAny<Domain.Entities.Category>()), Times.Once);
        }
    }
}
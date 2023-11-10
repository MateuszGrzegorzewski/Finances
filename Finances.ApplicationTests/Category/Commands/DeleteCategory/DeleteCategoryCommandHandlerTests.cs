using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using Moq;
using Xunit;

namespace Finances.Application.Category.Commands.DeleteCategory.Tests
{
    public class DeleteCategoryCommandHandlerTests
    {
        [Fact()]
        public async Task DeleteCategoryCommandHandler_DeleteCategory_WithAuthorizedUser()
        {
            var category = new Domain.Entities.Category()
            {
                Id = 1,
                CreatedById = "1"
            };

            var command = new DeleteCategoryCommand()
            {
                Name = "Home",
                EncodedName = "home"
            };

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("1", "test@example.com");
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(user);

            var categoyRepositoryMock = new Mock<ICategoryRepository>();
            categoyRepositoryMock.Setup(c => c.GetByEncodedName(command.EncodedName, user.Id))
                .ReturnsAsync(category);

            var handler = new DeleteCategoryCommandHandler(categoyRepositoryMock.Object, userContextMock.Object);

            await handler.Handle(command, CancellationToken.None);

            categoyRepositoryMock.Verify(m => m.Delete(It.IsAny<Domain.Entities.Category>()), Times.Once);
        }
    }
}
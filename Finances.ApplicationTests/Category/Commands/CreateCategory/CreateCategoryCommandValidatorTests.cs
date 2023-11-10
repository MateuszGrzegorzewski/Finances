using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Finances.Application.Category.Commands.CreateCategory.Tests
{
    public class CreateCategoryCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            var categoryName = "Home";
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@example.com"));

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(c => c.GetByName(categoryName, "1"));

            var validator = new CreateCategoryCommandValidator(categoryRepositoryMock.Object, userContextMock.Object);
            var command = new CreateCategoryCommand { Name = categoryName };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInValidCommand_ShouldHaveValidationErrors()
        {
            var category = new Domain.Entities.Category()
            {
                Name = "Test category",
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@example.com"));

            var categoyRepositoryMock = new Mock<ICategoryRepository>();
            categoyRepositoryMock.Setup(c => c.Create(category));

            var validator = new CreateCategoryCommandValidator(categoyRepositoryMock.Object, userContextMock.Object);
            var command = new CreateCategoryCommand()
            {
                Name = ""
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact()]
        public void ValidateCustomName_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            var categoryName = "Home";
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(e => e.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@example.com"));

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(c => c.GetByName(categoryName, "1"))
                .ReturnsAsync(new Domain.Entities.Category { Id = 2, Name = categoryName });

            var validator = new CreateCategoryCommandValidator(categoryRepositoryMock.Object, userContextMock.Object);
            var command = new CreateCategoryCommand { Name = categoryName };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Name);
        }
    }
}
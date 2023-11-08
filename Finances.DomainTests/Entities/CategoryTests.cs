using Xunit;
using FluentAssertions;

namespace Finances.Domain.Entities.Tests
{
    public class CategoryTests
    {
        [Fact()]
        public void EncodeName_ShouldSetEncodedName()
        {
            var category = new Category();
            category.Name = "Test Category";

            category.EncodeName();

            category.EncodedName.Should().Be("test-category");
        }

        [Fact()]
        public void EncodeName_shouldThrrowException_WhenNameIsNull()
        {
            var category = new Category();

            Action action = () => category.EncodeName();

            action.Invoking(a => a.Invoke())
                .Should().Throw<NullReferenceException>();
        }
    }
}
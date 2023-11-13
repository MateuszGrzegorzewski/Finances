using AutoMapper;
using Finances.Application.Category;
using FluentAssertions;
using Xunit;

namespace Finances.Application.Mappings.Tests
{
    public class CategoryMappingProfileTests
    {
        [Fact]
        public void ShouldMapCategoryDtoToCategory()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryMappingProfile>());
            var mapper = new Mapper(config);

            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Test Category",
                EncodedName = "test-category"
            };

            var category = mapper.Map<Domain.Entities.Category>(categoryDto);

            category.Should().NotBeNull();
            category.Id.Should().Be(categoryDto.Id);
            category.Name.Should().Be(categoryDto.Name);
            category.EncodedName.Should().Be(categoryDto.EncodedName);
        }

        [Fact]
        public void ShouldMapCategoryToCategoryDto()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryMappingProfile>());
            var mapper = new Mapper(config);

            var category = new Domain.Entities.Category
            {
                Id = 1,
                Name = "Test Category",
            };
            category.EncodeName();

            var categoryDto = mapper.Map<CategoryDto>(category);

            categoryDto.Should().NotBeNull();
            categoryDto.Id.Should().Be(category.Id);
            categoryDto.Name.Should().Be(category.Name);
            categoryDto.EncodedName.Should().Be(category.EncodedName);
        }
    }
}
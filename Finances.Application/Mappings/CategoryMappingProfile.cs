using AutoMapper;
using Finances.Application.Category;

namespace Finances.Application.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryDto, Domain.Entities.Category>();

            CreateMap<Domain.Entities.Category, CategoryDto>();
        }
    }
}
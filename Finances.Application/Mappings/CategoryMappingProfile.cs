using AutoMapper;
using Finances.Application.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
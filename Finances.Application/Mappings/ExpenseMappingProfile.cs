using AutoMapper;
using Finances.Application.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Mappings
{
    public class ExpenseMappingProfile : Profile
    {
        public ExpenseMappingProfile()
        {
            CreateMap<ExpenseDto, Domain.Entities.Expense>();

            CreateMap<Domain.Entities.Expense, ExpenseDto>();
        }
    }
}
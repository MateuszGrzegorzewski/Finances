using AutoMapper;
using Finances.Application.Expense;
using Finances.Application.Expense.Commands.EditExpense;

namespace Finances.Application.Mappings
{
    public class ExpenseMappingProfile : Profile
    {
        public ExpenseMappingProfile()
        {
            CreateMap<ExpenseDto, Domain.Entities.Expense>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<Domain.Entities.Expense, ExpenseDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<ExpenseDto, EditExpenseCommand>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
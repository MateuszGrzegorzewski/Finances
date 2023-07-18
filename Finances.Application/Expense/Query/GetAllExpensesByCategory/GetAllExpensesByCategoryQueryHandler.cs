using AutoMapper;
using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetAllExpensesByCategory
{
    public class GetAllExpensesByCategoryQueryHandler : IRequestHandler<GetAllExpensesByCategoryQuery, IEnumerable<ExpenseDto>>
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;

        public GetAllExpensesByCategoryQueryHandler(IExpenseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesByCategoryQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _repository.GetAllByCategory(request.Category);
            var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

            return dtos;
        }
    }
}
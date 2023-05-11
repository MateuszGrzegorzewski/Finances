using AutoMapper;
using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetAllExpenses
{
    public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseDto>>
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;

        public GetAllExpensesQueryHandler(IExpenseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _repository.GetAll();
            var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

            return dtos;
        }
    }
}
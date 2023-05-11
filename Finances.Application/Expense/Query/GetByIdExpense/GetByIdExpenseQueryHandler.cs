using AutoMapper;
using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetByIdExpense
{
    public class GetByIdExpenseQueryHandler : IRequestHandler<GetByIdExpenseQuery, ExpenseDto>
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;

        public GetByIdExpenseQueryHandler(IExpenseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExpenseDto> Handle(GetByIdExpenseQuery request, CancellationToken cancellationToken)
        {
            var expense = await _repository.GetById(request.Id);
            var dto = _mapper.Map<ExpenseDto>(expense);

            return dto;
        }
    }
}
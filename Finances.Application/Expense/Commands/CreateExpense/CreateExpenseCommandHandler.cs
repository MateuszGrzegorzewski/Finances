using AutoMapper;
using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = _mapper.Map<Domain.Entities.Expense>(request);

            await _expenseRepository.Create(expense);
        }
    }
}
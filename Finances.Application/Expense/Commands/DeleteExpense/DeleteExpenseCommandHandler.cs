using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.DeleteExpense
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
    {
        private readonly IExpenseRepository _repository;

        public DeleteExpenseCommandHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _repository.GetById(request.Id);

            await _repository.Delete(expense);
        }
    }
}
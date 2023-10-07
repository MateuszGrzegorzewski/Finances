using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.EditExpense
{
    public class EditExpenseCommandHandler : IRequestHandler<EditExpenseCommand>
    {
        private readonly IExpenseRepository _repository;

        public EditExpenseCommandHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(EditExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _repository.GetById(request.Id);

            expense.Value = request.Value;
            expense.Category = request.Category;
            expense.Description = request.Description;

            await _repository.Commit();
        }
    }
}
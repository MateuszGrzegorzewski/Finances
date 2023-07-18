using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory
{
    public class EditExpenseAfterDeletingCategoryCommandHandler : IRequestHandler<EditExpenseAfterDeletingCategoryCommand>
    {
        private readonly IExpenseRepository _repository;

        public EditExpenseAfterDeletingCategoryCommandHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(EditExpenseAfterDeletingCategoryCommand request, CancellationToken cancellationToken)
        {
            var expensesDto = request.ExpensesDto;

            foreach (var expenseDto in await expensesDto)
            {
                var expense = await _repository.GetById(expenseDto.Id);
                expense.Category = "undefined";
            }

            await _repository.Commit();
        }
    }
}
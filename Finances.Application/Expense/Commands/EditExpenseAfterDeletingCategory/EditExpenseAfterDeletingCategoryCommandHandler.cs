using Finances.Domain.Interfaces;
using MediatR;

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
                expense.Category = "Undefined";
            }

            await _repository.Commit();
        }
    }
}
using MediatR;

namespace Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory
{
    public class EditExpenseAfterDeletingCategoryCommand : IRequest
    {
        public Task<IEnumerable<ExpenseDto>> ExpensesDto { get; set; }

        public EditExpenseAfterDeletingCategoryCommand(Task<IEnumerable<ExpenseDto>> expensesDto)
        {
            ExpensesDto = expensesDto;
        }
    }
}
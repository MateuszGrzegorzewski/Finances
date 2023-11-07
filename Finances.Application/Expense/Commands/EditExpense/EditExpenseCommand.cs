using MediatR;

namespace Finances.Application.Expense.Commands.EditExpense
{
    public class EditExpenseCommand : ExpenseDto, IRequest
    {
    }
}
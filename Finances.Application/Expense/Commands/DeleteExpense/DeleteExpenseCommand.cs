using MediatR;

namespace Finances.Application.Expense.Commands.DeleteExpense
{
    public class DeleteExpenseCommand : ExpenseDto, IRequest
    {
    }
}
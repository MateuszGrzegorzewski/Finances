using MediatR;

namespace Finances.Application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommand : ExpenseDto, IRequest
    {
    }
}
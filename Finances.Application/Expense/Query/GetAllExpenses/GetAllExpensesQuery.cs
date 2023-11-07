using MediatR;

namespace Finances.Application.Expense.Query.GetAllExpenses
{
    public class GetAllExpensesQuery : IRequest<IEnumerable<ExpenseDto>>
    {
    }
}
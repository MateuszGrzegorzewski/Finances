using MediatR;

namespace Finances.Application.Expense.Query.GetAllExpensesByCategory
{
    public class GetAllExpensesByCategoryQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public string Category { get; set; }

        public GetAllExpensesByCategoryQuery(string category)
        {
            Category = category;
        }
    }
}
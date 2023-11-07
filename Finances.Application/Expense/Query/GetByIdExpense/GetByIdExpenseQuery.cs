using MediatR;

namespace Finances.Application.Expense.Query.GetByIdExpense
{
    public class GetByIdExpenseQuery : IRequest<ExpenseDto>
    {
        public int Id { get; set; }

        public GetByIdExpenseQuery(int id)
        {
            Id = id;
        }
    }
}
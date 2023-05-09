using Finances.Application.Expense;

namespace Finances.Application.Services
{
    public interface IExpenseService
    {
        Task Create(ExpenseDto expense);

        Task<IEnumerable<ExpenseDto>> GetAll();
    }
}
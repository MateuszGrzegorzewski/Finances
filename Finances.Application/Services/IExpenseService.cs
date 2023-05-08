using Finances.Domain.Entities;

namespace Finances.Application.Services
{
    public interface IExpenseService
    {
        Task Create(Expense expense);
    }
}
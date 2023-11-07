namespace Finances.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task Create(Entities.Expense expense);

        Task<IEnumerable<Entities.Expense>> GetAll(string currentUserId);

        Task<Entities.Expense> GetById(int id, string currentUserId);

        Task Commit();

        Task Delete(Entities.Expense expense);

        Task<IEnumerable<Entities.Expense>> GetAllByCategory(string category, string currentUserId);
    }
}
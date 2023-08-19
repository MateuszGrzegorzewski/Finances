//using Finances.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task Create(Entities.Expense expense);

        Task<IEnumerable<Entities.Expense>> GetAll();

        Task<Entities.Expense> GetById(int id);

        Task Commit();

        Task Delete(Entities.Expense expense);

        Task<IEnumerable<Entities.Expense>> GetAllByCategory(string category);
    }
}
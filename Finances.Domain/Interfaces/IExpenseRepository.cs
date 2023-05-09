using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task Create(Domain.Entities.Expense expense);

        Task<IEnumerable<Domain.Entities.Expense>> GetAll();
    }
}
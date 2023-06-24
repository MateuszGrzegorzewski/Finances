using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(Entities.Category category);

        Task<Entities.Category?> GetByName(string name);
    }
}
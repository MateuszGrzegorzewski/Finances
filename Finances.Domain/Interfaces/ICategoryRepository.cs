using Finances.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(Category category);

        Task<Category?> GetByName(string name);

        Task<IEnumerable<Category>> GetAll(string currentUserId);

        Task<Category> GetByEncodedName(string encodedName);

        Task Delete(Category category);
    }
}
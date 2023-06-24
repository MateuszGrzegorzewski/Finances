using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(Domain.Entities.Category category);
    }
}
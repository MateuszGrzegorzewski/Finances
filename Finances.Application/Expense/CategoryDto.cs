using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense
{
    public class CategoryDto
    {
        public string Name { get; set; } = default!;
        public string? EncodedName { get; set; }
    }
}
using Finances.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetAllExpensesByCategory
{
    public class GetAllExpensesByCategoryQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public string Category { get; set; }

        public GetAllExpensesByCategoryQuery(string category)
        {
            Category = category;
        }
    }
}
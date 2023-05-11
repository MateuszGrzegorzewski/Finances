using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetByIdExpense
{
    public class GetByIdExpenseQuery : IRequest<ExpenseDto>
    {
        public int Id { get; set; }

        public GetByIdExpenseQuery(int id)
        {
            Id = id;
        }
    }
}
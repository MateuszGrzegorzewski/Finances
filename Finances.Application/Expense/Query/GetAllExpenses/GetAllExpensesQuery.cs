using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetAllExpenses
{
    public class GetAllExpensesQuery : IRequest<IEnumerable<ExpenseDto>>
    {
    }
}
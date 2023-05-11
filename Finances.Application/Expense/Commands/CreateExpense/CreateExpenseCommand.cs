using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommand : ExpenseDto, IRequest
    {
    }
}
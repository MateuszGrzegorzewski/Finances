using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory
{
    public class EditExpenseAfterDeletingCategoryCommand : IRequest
    {
        public Task<IEnumerable<ExpenseDto>> ExpensesDto { get; set; }

        public EditExpenseAfterDeletingCategoryCommand(Task<IEnumerable<ExpenseDto>> expensesDto)
        {
            ExpensesDto = expensesDto;
        }
    }
}
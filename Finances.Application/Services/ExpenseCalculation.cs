using AutoMapper;
using Finances.Application.Expense.Query.GetAllExpenses;
using Finances.Application.Expense.Query.GetAllExpensesByCategory;
using Finances.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Services
{
    public class ExpenseCalculation
    {
        private readonly IMediator _mediator;

        public ExpenseCalculation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<decimal> totalExpensesByTerm(DateTime startDate, DateTime endDate)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            decimal totalExpenseInRange = expenses
                .Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate)
                .Sum(e => e.Value);

            return totalExpenseInRange;
        }

        public async Task<decimal> totalExpensesByYear(int year)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            var totalExpenseForYear = expenses
                .Where(e => e.CreatedAt.Year == year)
                .Sum(e => e.Value);

            return totalExpenseForYear;
        }

        public async Task<decimal> totalExpensesByLastMonths(int numOfLastMonths)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            var currentDate = DateTime.Now;

            var farthestDate = currentDate.AddMonths(-numOfLastMonths + 1);
            var totalExpenseForLastMonths = expenses
                .Where(e => e.CreatedAt >= new DateTime(farthestDate.Year, farthestDate.Month, 1))
                .Sum(e => e.Value);

            return totalExpenseForLastMonths;
        }
    }
}
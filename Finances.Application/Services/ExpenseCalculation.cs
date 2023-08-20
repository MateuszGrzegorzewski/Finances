using Finances.Application.Expense.Query.GetAllExpenses;
using MediatR;

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

            var totalExpenseInRange = expenses
                .Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate)
                .Sum(e => e.Value);

            return totalExpenseInRange;
        }

        public async Task<Dictionary<string, decimal>> totalExpensesByCategoryByTerm(DateTime startDate, DateTime endDate)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            var totalExpenseByCategoryInRangeDict = expenses
                .Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate)
                .GroupBy(e => e.Category)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(e => e.Value));

            return totalExpenseByCategoryInRangeDict;
        }

        public async Task<decimal> totalExpensesByYear(int year)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            var totalExpenseForYear = expenses
                .Where(e => e.CreatedAt.Year == year)
                .Sum(e => e.Value);

            return totalExpenseForYear;
        }

        public async Task<Dictionary<string, decimal>> totalExpensesByCategoryByYear(int year)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            var totalExpenseByCategoryByYearDict = expenses
                .Where(e => e.CreatedAt.Year == year)
                .GroupBy(e => e.Category)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(e => e.Value));

            return totalExpenseByCategoryByYearDict;
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

        public async Task<Dictionary<string, decimal>> totalExpensesByCategoryByLastMonths(int numOfLastMonths)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            var currentDate = DateTime.Now;
            var farthestDate = currentDate.AddMonths(-numOfLastMonths + 1);

            var totalExpenseByCategoryForLastMonthsDict = expenses
                .Where(e => e.CreatedAt >= new DateTime(farthestDate.Year, farthestDate.Month, 1))
                .GroupBy(e => e.Category)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(e => e.Value));

            return totalExpenseByCategoryForLastMonthsDict;
        }
    }
}
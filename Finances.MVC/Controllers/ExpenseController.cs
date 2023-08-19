using AutoMapper;
using Finances.Application.Expense;
using Finances.Application.Expense.Commands.CreateExpense;
using Finances.Application.Expense.Commands.DeleteExpense;
using Finances.Application.Expense.Commands.EditExpense;
using Finances.Application.Expense.Query.GetAllCategories;
using Finances.Application.Expense.Query.GetAllExpenses;
using Finances.Application.Expense.Query.GetAllExpensesByCategory;
using Finances.Application.Expense.Query.GetByIdExpense;
using Finances.Application.Services;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finances.MVC.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ExpenseCalculation _expenseCalculation;

        public ExpenseController(IMediator mediator, IMapper mapper, ExpenseCalculation expenseCalculation)
        {
            _mediator = mediator;
            _mapper = mapper;
            _expenseCalculation = expenseCalculation;
        }

        public async Task<IActionResult> Index(int? targetYear, int? targetNumOfMonths, DateTime startDate, DateTime endDate)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            if (targetYear.HasValue)
            {
                ViewBag.Year = targetYear.Value;

                var totalExpensesForYear = await _expenseCalculation.totalExpensesByYear(targetYear.Value);
                ViewBag.TotalExpensesForYear = totalExpensesForYear;

                var totalExpensesByCategoryForYearDict = await _expenseCalculation.totalExpensesByCategoryByYear(targetYear.Value);
                ViewBag.TotalExpenseByCategoryForYear = totalExpensesByCategoryForYearDict;
            }
            else
            {
                ViewBag.TotalExpenseByCategoryForYear = new Dictionary<string, decimal>();
            }

            if (targetNumOfMonths.HasValue)
            {
                ViewBag.NumOfLastMonths = targetNumOfMonths.Value;

                var totalExpensesForLastMonths = await _expenseCalculation.totalExpensesByLastMonths(targetNumOfMonths.Value);
                ViewBag.TotalExpensesForLastMonths = totalExpensesForLastMonths;

                var totalExpensesByCategoryForLastMonthsDict = await _expenseCalculation.totalExpensesByCategoryByLastMonths(targetNumOfMonths.Value);
                ViewBag.TotalExpensesByCategoryForLastMonths = totalExpensesByCategoryForLastMonthsDict;
            }
            else
            {
                ViewBag.TotalExpensesByCategoryForLastMonths = new Dictionary<string, decimal>();
            }

            ViewBag.StartDate = startDate.ToShortDateString();
            ViewBag.EndDate = endDate.ToShortDateString();

            var totalExpensesForTerm = await _expenseCalculation.totalExpensesByTerm(startDate, endDate);
            ViewBag.TotalExpensesForTerm = totalExpensesForTerm;

            var totalExpenseByCategoryInRangeDict = await _expenseCalculation.totalExpensesByCategoryByTerm(startDate, endDate);
            ViewBag.TotalExpenseByCategoryInRange = totalExpenseByCategoryInRangeDict;

            return View(expenses);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            ViewBag.Categories = categories;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _mediator.Send(new GetByIdExpenseQuery(id));
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            ViewBag.Categories = categories;

            var dto = await _mediator.Send(new GetByIdExpenseQuery(id));

            EditExpenseCommand model = _mapper.Map<EditExpenseCommand>(dto);

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _mediator.Send(new GetByIdExpenseQuery(id));

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseCommand command)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _mediator.Send(new GetAllCategoriesQuery());
                ViewBag.Categories = categories;

                return View();
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditExpenseCommand command)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _mediator.Send(new GetAllCategoriesQuery());
                ViewBag.Categories = categories;

                return View();
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, DeleteExpenseCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
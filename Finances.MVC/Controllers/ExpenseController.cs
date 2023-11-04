using AutoMapper;
using Finances.Application.Expense.Commands.CreateExpense;
using Finances.Application.Expense.Commands.DeleteExpense;
using Finances.Application.Expense.Commands.EditExpense;
using Finances.Application.Expense.Query.GetAllCategories;
using Finances.Application.Expense.Query.GetAllExpenses;
using Finances.Application.Expense.Query.GetByIdExpense;
using Finances.Application.Services;
using Finances.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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

        [Authorize]
        public async Task<IActionResult> Index(int? page, int? targetYear, int? targetNumOfMonths, DateTime startDate, DateTime endDate)
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());

            if (targetYear.HasValue)
            {
                ViewBag.SelectedYear = targetYear;

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
                ViewBag.SelectedNumMonths = targetNumOfMonths;

                var totalExpensesForLastMonths = await _expenseCalculation.totalExpensesByLastMonths(targetNumOfMonths.Value);
                ViewBag.TotalExpensesForLastMonths = totalExpensesForLastMonths;

                var totalExpensesByCategoryForLastMonthsDict = await _expenseCalculation.totalExpensesByCategoryByLastMonths(targetNumOfMonths.Value);
                ViewBag.TotalExpensesByCategoryForLastMonths = totalExpensesByCategoryForLastMonthsDict;
            }
            else
            {
                ViewBag.TotalExpensesByCategoryForLastMonths = new Dictionary<string, decimal>();
            }

            if (startDate > new DateTime(1, 1, 1) && endDate > new DateTime(1, 1, 1))
            {
                ViewBag.SelectedStartDate = startDate;
                ViewBag.SelectedEndDate = endDate;
                ViewBag.IsCalculateClicked = true;

                var totalExpensesForTerm = await _expenseCalculation.totalExpensesByTerm(startDate, endDate);
                ViewBag.TotalExpensesForTerm = totalExpensesForTerm;

                var totalExpenseByCategoryInRangeDict = await _expenseCalculation.totalExpensesByCategoryByTerm(startDate, endDate);
                ViewBag.TotalExpenseByCategoryInRange = totalExpenseByCategoryInRangeDict;
            }
            else
            {
                ViewBag.TotalExpenseByCategoryInRange = new Dictionary<string, decimal>();
                ViewBag.IsCalculateClicked = false;
            }

            int pageSize = 30;
            var pagedExpenses = expenses.ToPagedList(page ?? 1, pageSize);

            return View(pagedExpenses);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            ViewBag.Categories = categories;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var dto = await _mediator.Send(new GetByIdExpenseQuery(id));
                return View(dto);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("NoAccess", "Home");
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            ViewBag.Categories = categories;

            try
            {
                var dto = await _mediator.Send(new GetByIdExpenseQuery(id));

                EditExpenseCommand model = _mapper.Map<EditExpenseCommand>(dto);

                return View(model);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("NoAccess", "Home");
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dto = await _mediator.Send(new GetByIdExpenseQuery(id));
                return View(dto);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("NoAccess", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateExpenseCommand command)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _mediator.Send(new GetAllCategoriesQuery());
                ViewBag.Categories = categories;

                return View();
            }

            await _mediator.Send(command);

            this.SetNotification("success", $"Created expense: {command.Category} - {command.Value}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EditExpenseCommand command)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _mediator.Send(new GetAllCategoriesQuery());
                ViewBag.Categories = categories;

                return View();
            }

            await _mediator.Send(command);

            this.SetNotification("success", $"Edited expense: {command.Category} - {command.Value}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id, DeleteExpenseCommand command)
        {
            await _mediator.Send(command);

            this.SetNotification("info", $"Deleted expense: {command.Category} - {command.Value}");

            return RedirectToAction(nameof(Index));
        }
    }
}
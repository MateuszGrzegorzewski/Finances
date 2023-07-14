using AutoMapper;
using Finances.Application.Expense.Commands.CreateExpense;
using Finances.Application.Expense.Commands.DeleteExpense;
using Finances.Application.Expense.Commands.EditExpense;
using Finances.Application.Expense.Query.GetAllCategories;
using Finances.Application.Expense.Query.GetAllExpenses;
using Finances.Application.Expense.Query.GetByIdExpense;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finances.MVC.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ExpenseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());
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
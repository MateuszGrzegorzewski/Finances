﻿using AutoMapper;
using Finances.Application.Expense;
using Finances.Application.Expense.Commands.CreateExpense;
using Finances.Application.Expense.Query.GetAllExpenses;
using Finances.Application.Expense.Query.GetByIdExpense;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finances.MVC.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _mediator.Send(new GetAllExpensesQuery());
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        //[Route("Expense/{expenseId}/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _mediator.Send(new GetByIdExpenseQuery(id));
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
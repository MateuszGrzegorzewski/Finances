﻿using AutoMapper;
using Finances.Application.Expense.Commands.CreateCategory;
using Finances.Application.Expense.Commands.DeleteCategory;
using Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory;
using Finances.Application.Expense.Query.GetAllCategories;
using Finances.Application.Expense.Query.GetAllExpensesByCategory;
using Finances.Application.Expense.Query.GetByEncodedName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finances.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Route("Category/Delete/{encodedName}")]
        public async Task<IActionResult> Delete(string encodedName)
        {
            var dto = await _mediator.Send(new GetCategoryByEncodedNameQuery(encodedName));

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Category/Delete/{encodedName}")]
        public async Task<IActionResult> Delete(string encodedName, DeleteCategoryCommand command)
        {
            var dto = await _mediator.Send(new GetCategoryByEncodedNameQuery(encodedName));

            await _mediator.Send(command);

            var expenses = _mediator.Send(new GetAllExpensesByCategoryQuery(dto.Name));

            await _mediator.Send(new EditExpenseAfterDeletingCategoryCommand(expenses));

            return RedirectToAction(nameof(Index));
        }
    }
}
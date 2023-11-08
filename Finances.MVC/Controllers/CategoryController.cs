using AutoMapper;
using Finances.Application.Category.Commands.CreateCategory;
using Finances.Application.Category.Commands.DeleteCategory;
using Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory;
using Finances.Application.Category.Query.GetAllCategories;
using Finances.Application.Expense.Query.GetAllExpensesByCategory;
using Finances.Application.Category.Query.GetCategoryByEncodedName;
using Finances.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return View(categories);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Category/Delete/{encodedName}")]
        [Authorize]
        public async Task<IActionResult> Delete(string encodedName)
        {
            try
            {
                var dto = await _mediator.Send(new GetCategoryByEncodedNameQuery(encodedName));
                return View(dto);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("NoAccess", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _mediator.Send(command);

            this.SetNotification("success", $"Created category: {command.Name}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [Route("Category/Delete/{encodedName}")]
        public async Task<IActionResult> Delete(string encodedName, DeleteCategoryCommand command)
        {
            var dto = await _mediator.Send(new GetCategoryByEncodedNameQuery(encodedName));

            await _mediator.Send(command);

            var expenses = _mediator.Send(new GetAllExpensesByCategoryQuery(dto.Name));

            await _mediator.Send(new EditExpenseAfterDeletingCategoryCommand(expenses));

            this.SetNotification("info", $"Deleted category: {encodedName}");

            return RedirectToAction(nameof(Index));
        }
    }
}
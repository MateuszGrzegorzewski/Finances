using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Expense.Commands.EditExpenseAfterDeletingCategory
{
    public class EditExpenseAfterDeletingCategoryCommandHandler : IRequestHandler<EditExpenseAfterDeletingCategoryCommand>
    {
        private readonly IExpenseRepository _repository;
        private readonly IUserContext _userContext;

        public EditExpenseAfterDeletingCategoryCommandHandler(IExpenseRepository repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task Handle(EditExpenseAfterDeletingCategoryCommand request, CancellationToken cancellationToken)
        {
            var expensesDto = request.ExpensesDto;

            var currentUserId = _userContext.GetCurrentUser().Id;

            foreach (var expenseDto in await expensesDto)
            {
                var expense = await _repository.GetById(expenseDto.Id, currentUserId);
                expense.Category = "Undefined";
            }

            await _repository.Commit();
        }
    }
}
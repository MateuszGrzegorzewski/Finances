using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Expense.Commands.EditExpense
{
    public class EditExpenseCommandHandler : IRequestHandler<EditExpenseCommand>
    {
        private readonly IExpenseRepository _repository;
        private readonly IUserContext _userContext;

        public EditExpenseCommandHandler(IExpenseRepository repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task Handle(EditExpenseCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser().Id;

            var expense = await _repository.GetById(request.Id, currentUserId);

            expense.Value = request.Value;
            expense.Category = request.Category;
            expense.Description = request.Description;

            await _repository.Commit();
        }
    }
}
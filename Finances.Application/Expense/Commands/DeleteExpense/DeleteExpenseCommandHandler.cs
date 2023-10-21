using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Expense.Commands.DeleteExpense
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
    {
        private readonly IExpenseRepository _repository;
        private readonly IUserContext _userContext;

        public DeleteExpenseCommandHandler(IExpenseRepository repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser().Id;

            var expense = await _repository.GetById(request.Id, currentUserId);

            await _repository.Delete(expense);
        }
    }
}
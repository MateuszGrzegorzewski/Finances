using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IMapper mapper, IUserContext userContext)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = _mapper.Map<Domain.Entities.Expense>(request);

            expense.CreatedById = _userContext.GetCurrentUser().Id;

            await _expenseRepository.Create(expense);
        }
    }
}
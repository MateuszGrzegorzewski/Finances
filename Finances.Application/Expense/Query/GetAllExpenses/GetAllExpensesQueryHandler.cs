using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Expense.Query.GetAllExpenses
{
    public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseDto>>
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetAllExpensesQueryHandler(IExpenseRepository repository, IMapper mapper, IUserContext userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser().Id;

            var expenses = await _repository.GetAll(currentUserId);
            var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

            return dtos;
        }
    }
}
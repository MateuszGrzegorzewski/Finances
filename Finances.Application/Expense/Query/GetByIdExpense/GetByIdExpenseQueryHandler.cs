using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Expense.Query.GetByIdExpense
{
    public class GetByIdExpenseQueryHandler : IRequestHandler<GetByIdExpenseQuery, ExpenseDto>
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetByIdExpenseQueryHandler(IExpenseRepository repository, IMapper mapper, IUserContext userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<ExpenseDto> Handle(GetByIdExpenseQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser().Id;

            var expense = await _repository.GetById(request.Id, currentUserId);
            var dto = _mapper.Map<ExpenseDto>(expense);

            return dto;
        }
    }
}
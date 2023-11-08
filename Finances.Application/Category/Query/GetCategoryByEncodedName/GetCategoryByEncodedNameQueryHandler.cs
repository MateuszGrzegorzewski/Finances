using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Category;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Category.Query.GetCategoryByEncodedName
{
    public class GetCategoryByEncodedNameQueryHandler : IRequestHandler<GetCategoryByEncodedNameQuery, CategoryDto>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetCategoryByEncodedNameQueryHandler(ICategoryRepository repository, IMapper mapper, IUserContext userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<CategoryDto> Handle(GetCategoryByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser().Id;

            var category = await _repository.GetByEncodedName(request.EncodedName, currentUserId);

            var dto = _mapper.Map<CategoryDto>(category);

            return dto;
        }
    }
}
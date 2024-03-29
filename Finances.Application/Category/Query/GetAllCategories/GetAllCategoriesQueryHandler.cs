﻿using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Application.Category;
using Finances.Domain.Interfaces;
using MediatR;

namespace Finances.Application.Category.Query.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetAllCategoriesQueryHandler(ICategoryRepository repository, IMapper mapper, IUserContext userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser().Id;

            var categories = await _repository.GetAll(currentUserId);
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return dtos;
        }
    }
}
using AutoMapper;
using Finances.Application.Expense.Query.GetByEncodedName;
using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Query.GetCategoryByEncodedName
{
    public class GetCategoryByEncodedNameQueryHandler : IRequestHandler<GetCategoryByEncodedNameQuery, CategoryDto>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public GetCategoryByEncodedNameQueryHandler(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByEncodedName(request.EncodedName);

            var dto = _mapper.Map<CategoryDto>(category);

            return dto;
        }
    }
}
﻿using AutoMapper;
using Finances.Application.ApplicationUser;
using Finances.Domain.Entities;
using Finances.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Expense.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IUserContext userContext)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Entities.Category>(request);

            category.EncodeName();

            category.CreatedById = _userContext.GetCurrentUser().Id;

            await _categoryRepository.Create(category);
        }
    }
}
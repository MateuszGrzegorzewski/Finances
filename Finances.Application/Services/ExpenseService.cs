using AutoMapper;
using Finances.Application.Expense;
using Finances.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task Create(ExpenseDto expenseDto)
        {
            var expense = _mapper.Map<Domain.Entities.Expense>(expenseDto);

            await _expenseRepository.Create(expense);
        }

        public async Task<IEnumerable<ExpenseDto>> GetAll()
        {
            var expenses = await _expenseRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

            return dtos;
        }
    }
}
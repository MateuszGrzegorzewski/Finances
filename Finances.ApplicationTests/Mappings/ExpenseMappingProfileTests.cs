using AutoMapper;
using Finances.Application.Expense;
using Finances.Application.Expense.Commands.EditExpense;
using FluentAssertions;
using Xunit;

namespace Finances.Application.Mappings.Tests
{
    public class ExpenseMappingProfileTests
    {
        [Fact]
        public void ShouldMapExpenseDtoToExpense()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExpenseMappingProfile>());
            var mapper = new Mapper(config);

            var expenseDto = new ExpenseDto
            {
                Id = 1,
                Value = 100.50m,
                Category = "Test Category",
                CreatedAt = DateTime.UtcNow,
                Description = "Test Expense"
            };

            var expense = mapper.Map<Domain.Entities.Expense>(expenseDto);

            expense.Should().NotBeNull();
            expense.Id.Should().Be(expenseDto.Id);
            expense.Value.Should().Be(expenseDto.Value);
            expense.Category.Should().Be(expenseDto.Category);
            expense.CreatedAt.Should().Be(expenseDto.CreatedAt);
            expense.Description.Should().Be(expenseDto.Description);
        }

        [Fact]
        public void ShouldMapExpenseToExpenseDto()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExpenseMappingProfile>());
            var mapper = new Mapper(config);

            var expense = new Domain.Entities.Expense
            {
                Id = 1,
                Value = 100.50m,
                Category = "Test Category",
                CreatedAt = DateTime.UtcNow,
                Description = "Test Expense"
            };

            var expenseDto = mapper.Map<ExpenseDto>(expense);

            expenseDto.Should().NotBeNull();
            expenseDto.Id.Should().Be(expense.Id);
            expenseDto.Value.Should().Be(expense.Value);
            expenseDto.Category.Should().Be(expense.Category);
            expenseDto.CreatedAt.Should().Be(expense.CreatedAt);
            expenseDto.Description.Should().Be(expense.Description);
        }

        [Fact]
        public void ShouldMapExpenseDtoToEditExpenseCommand()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExpenseMappingProfile>());
            var mapper = new Mapper(config);

            var expenseDto = new ExpenseDto
            {
                Id = 1,
                Value = 100.50m,
                Category = "Test Category",
                CreatedAt = DateTime.UtcNow,
                Description = "Test Expense"
            };

            var editExpenseCommand = mapper.Map<EditExpenseCommand>(expenseDto);

            editExpenseCommand.Should().NotBeNull();
            editExpenseCommand.Id.Should().Be(expenseDto.Id);
            editExpenseCommand.Value.Should().Be(expenseDto.Value);
            editExpenseCommand.Category.Should().Be(expenseDto.Category);
            editExpenseCommand.CreatedAt.Should().Be(expenseDto.CreatedAt);
            editExpenseCommand.Description.Should().Be(expenseDto.Description);
        }
    }
}
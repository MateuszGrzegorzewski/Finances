using Finances.Application.Expense;
using Finances.Application.Expense.Query.GetAllExpenses;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Finances.Application.Services.Tests
{
    public class ExpenseCalculationTests
    {
        [Fact()]
        public async void totalExpensesByTerm_ReturnProperValue()
        {
            var expenses = new List<ExpenseDto>
                {
                    new ExpenseDto { Value = 10, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                    new ExpenseDto { Value = 20, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                    new ExpenseDto { Value = 30, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
                };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var expenseCalc = new ExpenseCalculation(mediatorMock.Object);

            var result = await expenseCalc.totalExpensesByTerm(new DateTime(2023, 9, 19), new DateTime(2023, 11, 19));

            result.Should().Be(60);
        }

        [Fact()]
        public async void totalExpensesByCategoryByTerm_ReturnProperValue()
        {
            var expenses = new List<ExpenseDto>
                {
                    new ExpenseDto { Value = 10, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                    new ExpenseDto { Value = 20, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                    new ExpenseDto { Value = 30, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
                };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var expenseCalc = new ExpenseCalculation(mediatorMock.Object);

            var result = await expenseCalc.totalExpensesByCategoryByTerm(new DateTime(2023, 9, 19), new DateTime(2023, 11, 19));

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainKey("Home").WhoseValue.Should().Be(30);
            result.Should().ContainKey("Food").WhoseValue.Should().Be(30);
        }

        [Fact()]
        public async void totalExpensesByYear_ReturnProperValue()
        {
            var expenses = new List<ExpenseDto>
                {
                    new ExpenseDto { Value = 10, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                    new ExpenseDto { Value = 20, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                    new ExpenseDto { Value = 30, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
                };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var expenseCalc = new ExpenseCalculation(mediatorMock.Object);

            var result = await expenseCalc.totalExpensesByYear(2023);

            result.Should().Be(60);
        }

        [Fact()]
        public async void totalExpensesByCategoryByYear_ReturnProperValue()
        {
            var expenses = new List<ExpenseDto>
                {
                    new ExpenseDto { Value = 10, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                    new ExpenseDto { Value = 20, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                    new ExpenseDto { Value = 30, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
                };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var expenseCalc = new ExpenseCalculation(mediatorMock.Object);

            var result = await expenseCalc.totalExpensesByCategoryByYear(2023);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainKey("Home").WhoseValue.Should().Be(30);
            result.Should().ContainKey("Food").WhoseValue.Should().Be(30);
        }

        [Fact()]
        public async void totalExpensesByLastMonths_ReturnProperValue()
        {
            var expenses = new List<ExpenseDto>
                {
                    new ExpenseDto { Value = 10, Category = "Home", CreatedAt = DateTime.Now.AddDays(-20) },
                    new ExpenseDto { Value = 20, Category = "Home", CreatedAt = DateTime.Now.AddDays(-15) },
                    new ExpenseDto { Value = 30, Category = "Food", CreatedAt = DateTime.Now.AddDays(-10) }
                };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var expenseCalc = new ExpenseCalculation(mediatorMock.Object);

            var result = await expenseCalc.totalExpensesByLastMonths(3);

            result.Should().Be(60);
        }

        [Fact()]
        public async void totalExpensesByCategoryByLastMonths_ReturnProperValue()
        {
            var expenses = new List<ExpenseDto>
                {
                    new ExpenseDto { Value = 10, Category = "Home", CreatedAt = DateTime.Now.AddDays(-20) },
                    new ExpenseDto { Value = 20, Category = "Home", CreatedAt = DateTime.Now.AddDays(-15) },
                    new ExpenseDto { Value = 30, Category = "Food", CreatedAt = DateTime.Now.AddDays(-10) }
                };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var expenseCalc = new ExpenseCalculation(mediatorMock.Object);

            var result = await expenseCalc.totalExpensesByCategoryByLastMonths(3);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainKey("Home").WhoseValue.Should().Be(30);
            result.Should().ContainKey("Food").WhoseValue.Should().Be(30);
        }
    }
}
using Finances.Application.ApplicationUser;
using Finances.Application.Expense;
using Finances.Application.Expense.Commands.CreateExpense;
using Finances.Application.Expense.Query.GetAllExpenses;
using Finances.Application.Expense.Query.GetByIdExpense;
using Finances.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Xunit;

namespace Finances.MVC.Controllers.Tests
{
    public class ExpenseControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ExpenseControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, "Test user"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Email, "test@example.com"),
                };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "TestScheme");

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }

        [Fact()]
        public async Task Index_ReturnView()
        {
            var client = _factory
             .WithWebHostBuilder(builder =>
             {
                 builder.ConfigureTestServices(services =>
                 {
                     services.AddAuthentication(defaultScheme: "TestScheme")
                         .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                             "TestScheme", options => { });
                 });
             })
             .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>");
        }

        [Fact()]
        public async Task Index_ExpensesCalculatedByYear()
        {
            // assert
            var expenses = new List<ExpenseDto>
            {
                new ExpenseDto { Value = 14, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                new ExpenseDto { Value = 19, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                new ExpenseDto { Value = 3, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense?targetYear=2023");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>")
                .And.Contain("<div class=\"text-end\">33</div>")
                .And.Contain("<div class=\"text-end\">3</div>")
                .And.Contain("<div>Total: </div>")
                .And.Contain("<div class=\"text-end\">36  </div>");
        }

        [Fact()]
        public async Task Index_ExpensesCalculatedByLastMonths()
        {
            // assert
            var expenses = new List<ExpenseDto>
            {
                new ExpenseDto { Value = 15, Category = "Home", CreatedAt = DateTime.Now.AddDays(-30) },
                new ExpenseDto { Value = 17, Category = "Home", CreatedAt = DateTime.Now.AddDays(-10) },
                new ExpenseDto { Value = 3, Category = "Food", CreatedAt = DateTime.Now.AddDays(-20) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense?targetNumOfMonths=3");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>")
                .And.Contain("<div class=\"text-end\">32</div>")
                .And.Contain("<div class=\"text-end\">3</div>")
                .And.Contain("<div>Total: </div>")
                .And.Contain("<div class=\"text-end\"> 35 </div>");
        }

        [Fact()]
        public async Task Index_ExpensesCalculatedByCustomDate()
        {
            // assert
            var expenses = new List<ExpenseDto>
            {
                new ExpenseDto { Value = 16, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                new ExpenseDto { Value = 17, Category = "Home", CreatedAt = new DateTime(2023, 11, 17) },
                new ExpenseDto { Value = 3, Category = "Food", CreatedAt = new DateTime(2023, 11, 12) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense?startDate=2023-11-02&endDate=2023-11-19");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>")
                .And.Contain("<div class=\"text-end\">33</div>")
                .And.Contain("<div class=\"text-end\">3</div>")
                .And.Contain("<div>Total: </div>")
                .And.Contain("<div class=\"text-end\">  36</div>");

            content.Should().Contain("Selected Start Date:")
                .And.Contain("Selected End Date:");
        }

        [Fact()]
        public async Task Index_ExpensesCalculatedNotByCustomDate_NotDisplayingSelectedStartAndEndDate()
        {
            // assert
            var expenses = new List<ExpenseDto>
            {
                new ExpenseDto { Value = 14, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                new ExpenseDto { Value = 19, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                new ExpenseDto { Value = 3, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense?targetYear=2023");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>");

            content.Should().NotContain("Selected Start Date:")
                .And.NotContain("Selected End Date:");
        }

        [Fact()]
        public async Task Index_ExpensesNotCalculated_NotDisplayingTotalExpenses()
        {
            // assert
            var expenses = new List<ExpenseDto>
            {
                new ExpenseDto { Value = 14, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                new ExpenseDto { Value = 19, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                new ExpenseDto { Value = 3, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>");

            content.Should().NotContain("<div>Total: </div>");
        }

        [Fact()]
        public async Task Index_DisplayingPagination()
        {
            // assert
            var expenses = new List<ExpenseDto>
            {
                new ExpenseDto { Value = 14, Category = "Home", CreatedAt = new DateTime(2023, 11, 19) },
                new ExpenseDto { Value = 19, Category = "Home", CreatedAt = new DateTime(2023, 10, 19) },
                new ExpenseDto { Value = 3, Category = "Food", CreatedAt = new DateTime(2023, 9, 19) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ExpenseDto>>(expenses));

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense?targetYear=2023");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Expenses</h1>");

            content.Should().Contain("<span class=\"page-link\" aria-hidden=\"true\">&laquo;&laquo;</span>")
                .And.Contain("<span class=\"page-link\" aria-hidden=\"true\">&raquo;</span>")
                .And.Contain("<a class=\"page-link\" href=\"/Expense?page=1\">1</a>");
        }

        [Fact()]
        public async Task Create_ReturnView()
        {
            // arrange
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync("/Expense/Create");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Add expense</h1>");
        }

        [Fact()]
        public async Task Create_CreatingExpense()
        {
            // arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateExpenseCommand>(), default))
                .Returns(Task.CompletedTask);

            var expenseRepositoryMock = new Mock<IExpenseRepository>();

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddScoped(_ => expenseRepositoryMock.Object);
                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    }))
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            var command = new CreateExpenseCommand()
            {
                Value = 15,
                Category = "Home"
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("/Expense/Create", jsonContent);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact()]
        public async Task Details_ReturnView()
        {
            // arrange
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var user = new CurrentUser("1", "test@example.com");

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(c => c.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => expenseRepositoryMock.Object);

                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync($"/Expense/Details/{expense.Id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h5 class=\"card-header\">Expense Details</h5>");
        }

        [Fact()]
        public async Task Edit_ReturnView()
        {
            // arrange
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var user = new CurrentUser("1", "test@example.com");

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(c => c.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => expenseRepositoryMock.Object);

                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync($"/Expense/Edit/{expense.Id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Edit expense</h1>");
        }

        [Fact()]
        public async Task Deatils_ReturnView_WithNoAccess()
        {
            // arrange
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetByIdExpenseQuery>(), default))
                .ThrowsAsync(new InvalidOperationException());

            var user = new CurrentUser("1", "test@example.com");

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(c => c.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddScoped(_ => expenseRepositoryMock.Object);

                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync($"/Expense/Details/{expense.Id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1 class=\"card-title text-danger\">You have no access to this resource</h1>");
        }

        [Fact()]
        public async Task Edit_ReturnView_WithNoAccess()
        {
            // arrange
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetByIdExpenseQuery>(), default))
                .ThrowsAsync(new InvalidOperationException());

            var user = new CurrentUser("1", "test@example.com");

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(c => c.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddScoped(_ => expenseRepositoryMock.Object);

                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync($"/Expense/Edit/{expense.Id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1 class=\"card-title text-danger\">You have no access to this resource</h1>");
        }

        [Fact()]
        public async Task Delete_ReturnsView()
        {
            // arrange
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8)
            };

            var user = new CurrentUser("1", "test@example.com");

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(c => c.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => expenseRepositoryMock.Object);

                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync($"/Expense/Delete/{expense.Id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h3>Are you sure you want to delete this?</h3>");
        }

        [Fact()]
        public async Task Delete_ReturnsView_WithNoAccess()
        {
            // arrange
            var expense = new Domain.Entities.Expense()
            {
                Id = 1,
                Value = 15,
                Category = "Home",
                CreatedAt = new DateTime(2023, 11, 8),
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetByIdExpenseQuery>(), default))
                .ThrowsAsync(new InvalidOperationException());

            var user = new CurrentUser("1", "test@example.com");

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(c => c.GetById(expense.Id, user.Id))
                .ReturnsAsync(expense);

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mediatorMock.Object);
                        services.AddScoped(_ => expenseRepositoryMock.Object);

                        services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(scheme: "TestScheme");

            // act
            var response = await client.GetAsync($"/Expense/Delete/{expense.Id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1 class=\"card-title text-danger\">You have no access to this resource</h1>");
        }
    }
}
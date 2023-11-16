using Finances.Application.ApplicationUser;
using Finances.Application.Category;
using Finances.Application.Category.Commands.CreateCategory;
using Finances.Application.Category.Query.GetAllCategories;
using Finances.Domain.Entities;
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
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CategoryControllerTests(WebApplicationFactory<Program> factory)
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
                var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "TestScheme");

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }

        [Fact()]
        public async Task Index_ReturnsViewWithCategories()
        {
            // arrange
            var categories = new List<CategoryDto>()
            {
                new CategoryDto()
                {
                    Name = "Home",
                },

                new CategoryDto()
                {
                    Name = "Food",
                },

                new CategoryDto()
                {
                    Name = "Health",
                }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

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
            var response = await client.GetAsync("/Category");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Categories</h1>")
                .And.Contain(@"<li class=""list-group-item"">Home</li>")
                .And.Contain(@"<li class=""list-group-item"">Food</li>")
                .And.Contain(@"<li class=""list-group-item"">Health</li>");
        }

        [Fact()]
        public async Task Index_ReturnsEmptyView_WhenNoCategoriesExist()
        {
            // arrange
            var categories = new List<CategoryDto>();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

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
            var response = await client.GetAsync("/Category");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().NotContain(@"<li class=""list-group-item"">");
        }

        [Fact()]
        public async Task Create_ReturnsView()
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
            var response = await client.GetAsync("/Category/Create");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Create Category</h1>");
        }

        //[Fact()]
        //public async Task Create_CreatingCategory()
        //{
        //    // arrange
        //    var mediatorMock = new Mock<IMediator>();

        //    mediatorMock.Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), default))
        //        .Returns(Task.CompletedTask);

        //    var userContextMock = new Mock<IUserContext>();

        //    userContextMock.Setup(e => e.GetCurrentUser())
        //        .Returns(new CurrentUser("1", "test@example.com"));

        //    var categoryRepositoryMock = new Mock<ICategoryRepository>();
        //    categoryRepositoryMock.Setup(r => r.GetByName(It.IsAny<string>(), It.IsAny<string>()))
        //                          .Returns(Task.FromResult<Domain.Entities.Category?>(null)); // Simulate no existing category

        //    var client = _factory
        //        .WithWebHostBuilder(builder =>
        //            builder.ConfigureTestServices(services =>
        //            {
        //                services.AddScoped(_ => mediatorMock.Object);
        //                services.AddScoped(_ => userContextMock.Object);
        //                services.AddScoped(_ => categoryRepositoryMock.Object);
        //            }))
        //        .WithWebHostBuilder(builder =>
        //        {
        //            builder.ConfigureTestServices(services =>
        //            {
        //                services.AddAuthentication(defaultScheme: "TestScheme")
        //                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
        //                        "TestScheme", options => { });
        //            });
        //        })
        //        .CreateClient();

        //    client.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue(scheme: "TestScheme");

        //    var command = new CreateCategoryCommand()
        //    {
        //        Name = "Home"
        //    };

        //    var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

        //    // act
        //    var response = await client.PostAsync("/Category/Create", jsonContent);

        //    // assert
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //    //response.Headers.Location.ToString().Should().EndWith("/Category/Index");

        //    mediatorMock.Verify(m => m.Send(It.IsAny<CreateCategoryCommand>(), default), Times.Once);
        //}

        // Testing Delete Category
    }
}
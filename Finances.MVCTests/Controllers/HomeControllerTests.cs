using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace Finances.Controllers.Tests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HomeControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact()]
        public async Task Index_ReturnsView()
        {
            // arrange

            var client = _factory.CreateClient();

            // act

            var response = await client.GetAsync("/Home");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h2 class=\"display-3\">Welcome to Finances</h2>");
        }

        [Fact()]
        public async Task NoAccess_ReturnsView()
        {
            // arrange

            var client = _factory.CreateClient();

            // act

            var response = await client.GetAsync("/Home/NoAccess");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("You have no access to this resource");
        }
    }
}
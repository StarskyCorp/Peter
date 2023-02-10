using Api.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class ModuleShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public ModuleShould(WebApplicationFactory<IApiMarker> factory)
    {
        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task return_customers()
    {
        (await _client.GetStringAsync("/customers")).Should().Be("Customers");
    }

    [Fact]
    public async Task return_users()
    {
        (await _client.GetStringAsync("/users")).Should().Be("Users");
    }
}
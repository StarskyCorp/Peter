using System.Net;
using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class NotAllowedShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public NotAllowedShould(WebApplicationFactory<IApiMarker> app)
    {
        _client = app.CreateDefaultClient();
    }

    [Fact]
    public async Task return_forbidden()
    {
        var response = await _client.GetAsync("not_allowed");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
}
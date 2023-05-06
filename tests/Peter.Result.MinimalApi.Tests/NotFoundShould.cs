using System.Net;
using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class NotFoundShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public NotFoundShould(WebApplicationFactory<IApiMarker> app)
    {
        _client = app.CreateDefaultClient();
    }

    [Fact]
    public async Task return_not_found()
    {
        var response = await _client.GetAsync("not_found");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
}
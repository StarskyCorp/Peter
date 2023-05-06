using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class TeapotShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public TeapotShould(WebApplicationFactory<IApiMarker> app)
    {
        _client = app.CreateDefaultClient();
    }

    [Fact]
    public async Task return_a_teapot()
    {
        var response = await _client.GetAsync("teapot?ok=true&owner=Peter");

        ((int)response.StatusCode).Should().Be(418);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("I'm Peter's teapot");
    }

    [Fact]
    public async Task return_a_no_teapot()
    {
        var response = await _client.GetAsync("teapot?ok=false&owner=Peter");

        ((int)response.StatusCode).Should().Be(418);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("I'm not Peter's teapot");
    }

    [Fact]
    public async Task return_aged_teapot()
    {
        var response = await _client.GetAsync("aged_teapot?age=47");

        ((int)response.StatusCode).Should().Be(418);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("I'm a 47 teapot years old");
    }
}
using System.Net;
using System.Net.Http.Json;
using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class OkShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;
    private readonly string _baseAddress;

    public OkShould(WebApplicationFactory<IApiMarker> app)
    {
        _client = app.CreateDefaultClient();
        _baseAddress = app.Server.BaseAddress.ToString().TrimEnd('/');
    }

    [Fact]
    public async Task return_ok()
    {
        var response = await _client.GetAsync("ok");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_created()
    {
        var response = await _client.PostAsync("created", null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be("/peter");
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_created_at_route()
    {
        var response = await _client.PostAsync("created_at_route", null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"{_baseAddress}/peter/1");
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_accepted()
    {
        var response = await _client.PostAsync("accepted", null);

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        response.Headers.Location.Should().Be("/peter");
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_accepted_at_route()
    {
        var response = await _client.PostAsync("accepted_at_route", null);

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        response.Headers.Location.Should().Be($"{_baseAddress}/peter/1");
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_ok_using_very_ok_type()
    {
        var response = await _client.GetAsync("ok_using_very_ok_type");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_ok_using_result_type()
    {
        var response = await _client.GetAsync("ok_using_result_type");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_ok_with_no_value()
    {
        var response = await _client.GetAsync("ok_with_no_value");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
}
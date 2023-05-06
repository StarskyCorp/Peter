using System.Net;
using System.Net.Http.Json;
using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class ErrorShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public ErrorShould(WebApplicationFactory<IApiMarker> app)
    {
        _client = app.CreateDefaultClient();
    }

    [Fact]
    public async Task return_an_error()
    {
        var response = await _client.GetAsync("error");

        var problemDetails = (await response.Content.ReadFromJsonAsync<ProblemDetails>())!;
        problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Title.Should().Be("An error occurred while processing your request.");
        problemDetails.Detail.Should().Be("Peter");
    }

    [Fact]
    public async Task return_an_error_without_detail()
    {
        var response = await _client.GetAsync("error_without_detail");

        var problemDetails = (await response.Content.ReadFromJsonAsync<ProblemDetails>())!;
        problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Title.Should().Be("An error occurred while processing your request.");
        problemDetails.Detail.Should().BeNull();
    }

    [Fact]
    public async Task return_an_internal_server_error()
    {
        var response = await _client.GetAsync("internal_server_error");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadFromJsonAsync<string>()).Should().Be("Peter");
    }

    [Fact]
    public async Task return_an_internal_server_error_without_body()
    {
        var response = await _client.GetAsync("internal_server_error_without_body");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task return_an_error_using_result_type_without_body()
    {
        var response = await _client.GetAsync("error_using_result_type_without_body");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
}
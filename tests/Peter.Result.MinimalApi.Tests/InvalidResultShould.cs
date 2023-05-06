using System.Net;
using System.Net.Http.Json;
using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class InvalidResultShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public InvalidResultShould(WebApplicationFactory<IApiMarker> app)
    {
        _client = app.CreateDefaultClient();
    }

    [Fact]
    public async Task return_bad_request_using_problem_details_with_simple_invalid_result_type()
    {
        var response = await _client.GetAsync("bad_request_using_problem_details_with_simple_invalid_result_type");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails =
            (await response.Content.ReadFromJsonAsync<ProblemDetails>())!;
        problemDetails.Title.Should().Be("An error occurred while processing your request.");
        problemDetails.Detail.Should().Be("Peter");
    }

    [Fact]
    public async Task return_bad_request_using_validation_problem_details_with_detailed_invalid_result_type()
    {
        var response =
            await _client.GetAsync("bad_request_using_validation_problem_details_with_detailed_invalid_result_type");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var httpValidationProblemDetails =
            (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        httpValidationProblemDetails.Errors.Single().Should()
            .BeEquivalentTo(new KeyValuePair<string, string[]>("Peter", new[] { "Starsky" }));
    }

    [Fact]
    public async Task return_bad_request_with_simple_invalid_result_type()
    {
        var response = await _client.GetAsync("bad_request_with_simple_invalid_result_type");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadFromJsonAsync<string>())!.Should().Be("Peter");
    }

    [Fact]
    public async Task return_bad_request_with_detailed_invalid_result_type()
    {
        var response = await _client.GetAsync("bad_request_with_detailed_invalid_result_type");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var detailedMessages = await response.Content.ReadFromJsonAsync<IEnumerable<DetailedMessage>>();
        detailedMessages.Should().BeEquivalentTo(new DetailedMessage[]
            { new("Peter", new string[] { "Starsky" }) });
    }
}
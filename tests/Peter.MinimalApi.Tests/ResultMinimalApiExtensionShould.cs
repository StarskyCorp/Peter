using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GreetingsApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class ResultMinimalApiExtensionShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public ResultMinimalApiExtensionShould(WebApplicationFactory<IApiMarker> app) => _client = app.CreateDefaultClient();

    [Fact]
    public async Task return_ok()
    {
        var response = await _client.GetAsync("ok");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        // TODO Be explicit about content type has to be equal to application/json
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_failed_using_problem_details()
    {
        var response = await _client.GetAsync("failed");
        var details = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
        details.Status.Should().Be(StatusCodes.Status500InternalServerError);
        details.Title.Should().Be("Error");
        details.Detail.Should().Be("A failure");
    }

    [Fact]
    public async Task return_failed()
    {
        var response = await _client.GetAsync("failed_no_problem_details");
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task return_not_exists()
    {
        var response = await _client.GetAsync("not_exists");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task return_not_exists_with_value()
    {
        var response = await _client.GetAsync("not_exists_with_value");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_invalid()
    {
        var response = await _client.GetAsync("invalid");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        content.Errors.Single().Should().BeEquivalentTo(new KeyValuePair<string, string[]>("peter", new[] { "message" }));
    }
}
using System.Net;
using System.Net.Http.Json;
using Api.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Peter.Result.MinimalApi.Tests;

public class ResultExtensionsShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly WebApplicationFactory<IApiMarker> _app;
    private readonly HttpClient _client;

    public ResultExtensionsShould(WebApplicationFactory<IApiMarker> app)
    {
        _app = app;
        _client = _app.CreateDefaultClient();
    }

    [Fact]
    public async Task return_ok()
    {
        HttpResponseMessage response = await _client.GetAsync("ok");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_created_at_with_complete_route_info()
    {
        var response = await _client.PostAsJsonAsync("created_at", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var baseAddress = _app.Server.BaseAddress.ToString().TrimEnd('/');
        response.Headers.Location.Should().Be($"{baseAddress}/foo/1");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_created()
    {
        var response = await _client.PostAsJsonAsync("created", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be("/anyUrl");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_accepted_at_with_complete_route_info()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("accepted_at", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var baseAddress = _app.Server.BaseAddress.ToString().TrimEnd('/');
        response.Headers.Location.Should().Be($"{baseAddress}/foo/2");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_accepted()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("accepted", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        response.Headers.Location.Should().Be("/anyUrl");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_failed_using_problem_details()
    {
        HttpResponseMessage response = await _client.GetAsync("failed_using_problem_details");
        ProblemDetails details =
            JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync())!;
        details.Status.Should().Be(StatusCodes.Status500InternalServerError);
        details.Title.Should().Be("Error");
        details.Detail.Should().Be("A failure");
    }

    [Fact]
    public async Task return_failed_not_using_problem_details()
    {
        HttpResponseMessage response = await _client.GetAsync("failed_not_using_problem_details");
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task return_not_found()
    {
        HttpResponseMessage response = await _client.GetAsync("not_exists");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task return_not_found_with_value()
    {
        HttpResponseMessage response = await _client.GetAsync("not_exists_with_value");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_no_content()
    {
        HttpResponseMessage response = await _client.GetAsync("no_content");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task return_invalid()
    {
        HttpResponseMessage response = await _client.GetAsync("invalid");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        HttpValidationProblemDetails content =
            (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        content.Errors.Single().Should()
            .BeEquivalentTo(new KeyValuePair<string, string[]>("peter", new[] { "message" }));
    }
}
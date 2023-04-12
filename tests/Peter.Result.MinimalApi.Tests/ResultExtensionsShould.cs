using System.Net;
using System.Net.Http.Json;
using Api;
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
        var response = await _client.GetAsync("ok");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_created()
    {
        var response = await _client.PostAsJsonAsync("created", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be("/any_url");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_created_at_route()
    {
        var response = await _client.PostAsJsonAsync("created_at_route", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var baseAddress = _app.Server.BaseAddress.ToString().TrimEnd('/');
        response.Headers.Location.Should().Be($"{baseAddress}/foo/1");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_accepted()
    {
        var response = await _client.PostAsJsonAsync("accepted", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        response.Headers.Location.Should().Be("/any_url");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_accepted_at_route()
    {
        var response = await _client.PostAsJsonAsync("accepted_at_route", "foo");

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var baseAddress = _app.Server.BaseAddress.ToString().TrimEnd('/');
        response.Headers.Location.Should().Be($"{baseAddress}/foo/2");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_very_ok()
    {
        var response = await _client.GetAsync("very_ok");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_problem()
    {
        var response = await _client.GetAsync("problem");

        var details =
            JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync())!;
        var statusCode = (HttpStatusCode)details.Status!;
        statusCode.Should().Be(HttpStatusCode.InternalServerError);
        details.Title.Should().Be("Error");
        details.Detail.Should().Be("A failure");
    }

    [Fact]
    public async Task return_internal_server_error()
    {
        var response = await _client.GetAsync("internal_server_error");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("A failure");
    }

    [Fact]
    public async Task return_not_found()
    {
        var response = await _client.GetAsync("not_found");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_no_content()
    {
        var response = await _client.GetAsync("no_content");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task return_validation_problem()
    {
        var response = await _client.GetAsync("validation_problem");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content =
            (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        content.Errors.Single().Should()
            .BeEquivalentTo(new KeyValuePair<string, string[]>("peter", new[] { "message" }));
    }

    [Fact]
    public async Task return_bad_request()
    {
        var response = await _client.GetAsync("bad_request?simple=false");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content =
            (await response.Content.ReadFromJsonAsync<IEnumerable<ValidationError>>())!;
        content.Single().Should()
            .BeEquivalentTo(new ValidationError("peter", "message"));
    }

    [Fact]
    public async Task return_simple_bad_request()
    {
        var response = await _client.GetAsync("bad_request?simple=true");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content =
            (await response.Content.ReadFromJsonAsync<IEnumerable<string>>())!;
        content.Single().Should()
            .BeEquivalentTo("message");
    }

    [Fact]
    public async Task return_open_teapot()
    {
        var response = await _client.GetAsync("open_teapot?ok=true");

        ((int)response.StatusCode).Should().Be(418);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("I'm Peter's teapot");
    }

    [Fact]
    public async Task return_open_no_teapot()
    {
        var response = await _client.GetAsync("open_teapot?ok=false");

        ((int)response.StatusCode).Should().Be(418);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("I'm not Peter's teapot");
    }

    [Fact]
    public async Task return_closed_teapot()
    {
        var response = await _client.GetAsync("closed_teapot?age=47");

        ((int)response.StatusCode).Should().Be(418);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("I'm a 47 teapot year old");
    }

    [Fact]
    public async Task return_ok_using_result_type_base()
    {
        var response = await _client.GetAsync("using_result_type_base?ok=true&toString=false");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Be("\"Peter\"");
    }

    [Fact]
    public async Task return_error_using_result_type_base()
    {
        var response = await _client.GetAsync("using_result_type_base?ok=false&toString=false");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
    
    [Fact]
    public async Task return_error_using_result_type_base_to_string()
    {
        var response = await _client.GetAsync("using_result_type_base?ok=false&toString=true");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Peter.Result.Result`1[System.Object]");
    }
}
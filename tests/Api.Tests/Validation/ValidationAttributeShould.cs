using System.Net;
using System.Net.Http.Json;
using Api.Features.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Tests.Validation;

public class ValidationAttributeShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public ValidationAttributeShould(WebApplicationFactory<IApiMarker> factory) =>
        _client = factory.CreateDefaultClient();

    [Fact]
    public async Task validate_successfully_when_data_is_valid() =>
        (await _client.PostAsJsonAsync("validate_using_validate_attribute", new Product { Id = 1, Name = "A product" }))
        .StatusCode.Should().Be(HttpStatusCode.OK);

    [Fact]
    public async Task fail_validation_when_data_is_invalid()
    {
        var response =
            await _client.PostAsJsonAsync("validate_using_validate_attribute", new Product { Id = 0, Name = null });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        content.Errors.Should().HaveCount(2);
        content.Errors["Id"].Should().BeEquivalentTo("'Id' must be greater than '0'.");
        content.Errors["Name"].Should().BeEquivalentTo("'Name' must not be empty.");
    }

    [Fact]
    public async Task fail_validation_when_validate_attribute_is_omitted_and_data_is_invalid()
    {
        var response = await _client.PostAsJsonAsync("validate_omitting_validate_attribute",
            new Product { Id = 0, Name = null });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        content.Errors.Should().HaveCount(2);
        content.Errors["Id"].Should().BeEquivalentTo("'Id' must be greater than '0'.");
        content.Errors["Name"].Should().BeEquivalentTo("'Name' must not be empty.");
    }
}
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GreetingsApi;
using GreetingsApi.Features.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class ValidationFilterShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public ValidationFilterShould(WebApplicationFactory<IApiMarker> factory)
    {
        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task validate_with_fail_when_data_is_not_valid()
    {
        var response = await _client.PostAsJsonAsync("validate_using_attribute", new Product { Id = 0, Name = null });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        content.Errors.Should().HaveCount(2);
        content.Errors["Id"].Should().BeEquivalentTo("'Id' must be greater than '0'.");
        content.Errors["Name"].Should().BeEquivalentTo("'Name' must not be empty.");
    }

    [Fact]
    public async Task validate_successfully_when_data_is_valid()
    {
        (await _client.PostAsJsonAsync("validate_using_attribute", new Product { Id = 1, Name = "A product" }))
            .StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task fail_when_there_is_not_a_custom_validator_registered()
    {
        var response = await _client.PostAsJsonAsync(
            "fail_validation_using_attribute_when_there_is_not_a_custom_validator_registered",
            new ProductWithoutCustomValidator { Id = 0, Name = null });

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await response.Content.ReadAsStringAsync();
        content.Should()
            .StartWith(
                "System.InvalidOperationException: No service for type 'FluentValidation.IValidator`1[GreetingsApi.Features.Validation.ProductWithoutCustomValidator]' has been registered.");
    }
}
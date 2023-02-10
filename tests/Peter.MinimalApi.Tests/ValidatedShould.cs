﻿using System.Net;
using System.Net.Http.Json;
using Api.Tests;
using Api.Tests.Features.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class ValidatedShould : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;

    public ValidatedShould(WebApplicationFactory<IApiMarker> factory)
    {
        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task validate_with_fail_when_data_is_not_valid()
    {
        var response = await _client.PostAsJsonAsync("validate_using_validated", new Product { Id = 0, Name = null });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())!;
        content.Errors.Should().HaveCount(2);
        content.Errors["Id"].Should().BeEquivalentTo("'Id' must be greater than '0'.");
        content.Errors["Name"].Should().BeEquivalentTo("'Name' must not be empty.");
    }

    [Fact]
    public async Task validate_successfully_when_data_is_valid()
    {
        (await _client.PostAsJsonAsync("validate_using_validated", new Product { Id = 1, Name = "A product" }))
            .StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task fail_when_value_is_null()
    {
        var response = await _client.PostAsJsonAsync<Product>("validate_using_validated", null);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync()).Should()
            .StartWith("System.ArgumentException: product cannot be null.");
    }

    [Fact]
    public async Task fail_when_there_is_not_a_custom_validator_registered()
    {
        var response = await _client.PostAsJsonAsync(
            "fail_validation_using_validated_when_there_is_not_a_custom_validator_registered",
            new ProductWithoutCustomValidator { Id = 0, Name = null });

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync()).Should()
            .StartWith(
                "System.InvalidOperationException: No service for type 'FluentValidation.IValidator`1[Api.Tests.Features.Validation.ProductWithoutCustomValidator]' has been registered.");
    }
}
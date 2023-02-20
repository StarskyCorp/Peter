﻿using System.Net;
using System.Security.Claims;
using Api.Tests;
using FluentAssertions;
using Peter.MinimalApi.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

[CollectionDefinition(nameof(DatabaseCollectionFixture))]
public class DatabaseCollectionFixture : ICollectionFixture<ServerFixture<IApiMarker>>
{
}

[Collection(nameof(DatabaseCollectionFixture))]
public class ServerFixtureInCollectionFixtureShould
{
    private readonly ServerFixture<IApiMarker> _fixture;

    public ServerFixtureInCollectionFixtureShould(ServerFixture<IApiMarker> fixture) => _fixture = fixture;

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task greet_non_authenticated()
    {
        HttpResponseMessage response = await _fixture.Client().GetAsync("/Peter/authenticated");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task greet_authenticated()
    {
        var claims = new Claim[] { new(ClaimTypes.Name, "Peter") };
        var response = await _fixture.AuthenticatedClient(claims).GetStringAsync("/Peter/authenticated");
        response.Should().StartWith("Hello Peter!");
        response.Should().EndWith(string.Join<Claim>(",", claims));
    }

    [Fact]
    public async Task invoke_initialize_method()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
    }
}
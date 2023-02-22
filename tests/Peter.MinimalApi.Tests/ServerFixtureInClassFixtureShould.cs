﻿using System.Net;
using System.Security.Claims;
using Api.Tests;
using FluentAssertions;
using Peter.MinimalApi.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Peter.MinimalApi.Tests;

public class ServerFixtureInClassFixtureShould : IClassFixture<ServerFixture<IApiMarker>>
{
    private readonly ServerFixture<IApiMarker> _fixture;

    public ServerFixtureInClassFixtureShould(ServerFixture<IApiMarker> fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        fixture.RegisterLoggerProvider(output);
    }

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task greet_non_authenticated()
    {
        HttpResponseMessage? response = await _fixture.Client().GetAsync("/Peter/authenticated");
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
    public async Task show_api_logs()
    {
        var response = await _fixture.Client().GetStringAsync("/users/log");
        response.Should().StartWith("Users");
    }
}
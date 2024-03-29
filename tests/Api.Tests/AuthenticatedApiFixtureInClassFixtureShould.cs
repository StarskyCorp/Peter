﻿using System.Net;
using System.Security.Claims;
using FluentAssertions;
using Peter.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests;

public class AuthenticatedApiFixtureInClassFixtureShould : IClassFixture<AuthenticatedApiFixture<IApiMarker>>
{
    private readonly AuthenticatedApiFixture<IApiMarker> _fixture;

    public AuthenticatedApiFixtureInClassFixtureShould(AuthenticatedApiFixture<IApiMarker> fixture,
        ITestOutputHelper output)
    {
        _fixture = fixture;
        fixture.RegisterLoggerProvider(output);
    }

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.CreateDefaultClient().GetStringAsync("/Peter");

        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task greet_non_authenticated()
    {
        var response = await _fixture.CreateDefaultClient().GetAsync("/Peter/authenticated");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task greet_authenticated()
    {
        var claims = new Claim[] { new(ClaimTypes.Name, "Peter") };

        var response = await _fixture.CreateAuthenticatedClient(claims).GetStringAsync("/Peter/authenticated");

        response.Should().StartWith("Hello Peter!");
        response.Should().EndWith(string.Join<Claim>(",", claims));
    }

    [Fact]
    public async Task show_api_logs()
    {
        var response = await _fixture.CreateDefaultClient().GetStringAsync("/users/log");

        response.Should().Be("Users");
    }
}
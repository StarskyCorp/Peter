﻿using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Peter.Testing;
using Xunit;

namespace Peter.Tests;

public class GreetingsApiShould : IClassFixture<ServerFixture<Program>>
{
    private readonly ServerFixture<Program> _fixture;

    public GreetingsApiShould(ServerFixture<Program> fixture) => _fixture = fixture;

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.Client.GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task get_401_for_not_authenticated_client()
    {
        var response = await _fixture.Client.GetAsync("/Peter/Authenticated");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task greet_authenticated()
    {
        var claims = new Claim[] { new(ClaimTypes.Name, "Peter") };
        var response = await _fixture.AuthenticatedClient(claims).GetStringAsync("/Peter/Authenticated");
        response.Should().StartWith("Hello Peter!");
        response.Should().EndWith(string.Join<Claim>(",", claims));
    }
}
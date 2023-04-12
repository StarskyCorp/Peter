using System.Net;
using System.Security.Claims;
using FluentAssertions;
using Peter.Testing;
using Xunit;

namespace Api.Tests;

[CollectionDefinition(nameof(DatabaseCollectionFixture))]
public class DatabaseCollectionFixture : ICollectionFixture<AuthenticatedApiFixture<IApiMarker>>
{
}

[Collection(nameof(DatabaseCollectionFixture))]
public class AuthenticatedApiFixtureInCollectionFixtureShould
{
    private readonly AuthenticatedApiFixture<IApiMarker> _fixture;

    public AuthenticatedApiFixtureInCollectionFixtureShould(AuthenticatedApiFixture<IApiMarker> fixture) =>
        _fixture = fixture;

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");

        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task greet_non_authenticated()
    {
        var response = await _fixture.Client().GetAsync("/Peter/authenticated");

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
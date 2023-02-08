using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Tests;
using FluentAssertions;
using Peter.MinimalApi.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class ServerFixtureInClassFixtureShould : IClassFixture<ServerFixture<IApiMarker>>
{
    private readonly ServerFixture<IApiMarker> _fixture;

    public ServerFixtureInClassFixtureShould(ServerFixture<IApiMarker> fixture) => _fixture = fixture;

    [Fact]
    public async Task greet()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
    }

    [Fact]
    public async Task greet_non_authenticated()
    {
        var response = await _fixture.Client().GetAsync("/Peter/Authenticated");
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
using FluentAssertions;
using Moq;
using Peter.Testing;
using Xunit;

namespace Api.Tests;

public class AuthenticatedApiFixtureInitializer : IAuthenticatedApiFixtureInitializer
{
    public static readonly Mock<IAuthenticatedApiFixtureInitializer> Initializer;

    static AuthenticatedApiFixtureInitializer() => Initializer = new Mock<IAuthenticatedApiFixtureInitializer>();

    public void Initialize(IServiceProvider services) => Initializer.Object.Initialize(services);
}

public class
    AuthenticatedApiFixtureInitializerShould : IClassFixture<
        AuthenticatedApiFixture<IApiMarker, AuthenticatedApiFixtureInitializer>>
{
    private readonly AuthenticatedApiFixture<IApiMarker, AuthenticatedApiFixtureInitializer> _fixture;

    public AuthenticatedApiFixtureInitializerShould(
        AuthenticatedApiFixture<IApiMarker, AuthenticatedApiFixtureInitializer> fixture) =>
        _fixture = fixture;

    [Fact]
    public async Task be_invoked_when_the_test_server_is_created()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");

        response.Should().Be("Hello Peter!");
        AuthenticatedApiFixtureInitializer.Initializer.Verify(a => a.Initialize(It.IsAny<IServiceProvider>()));
    }
}
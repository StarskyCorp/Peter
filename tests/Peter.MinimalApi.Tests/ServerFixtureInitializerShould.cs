using Api.Tests;
using FluentAssertions;
using Moq;
using Peter.MinimalApi.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class ServerFixtureInitializer : IServerFixtureInitializer
{
    public static readonly Mock<IServerFixtureInitializer> _initializer;

    static ServerFixtureInitializer() => _initializer = new Mock<IServerFixtureInitializer>();

    public void Initialize(IServiceProvider services) => _initializer.Object.Initialize(services);
}

public class ServerFixtureInitializerShould : IClassFixture<ServerFixture<IApiMarker, ServerFixtureInitializer>>
{
    private readonly ServerFixture<IApiMarker, ServerFixtureInitializer> _fixture;

    public ServerFixtureInitializerShould(ServerFixture<IApiMarker, ServerFixtureInitializer> fixture) =>
        _fixture = fixture;

    [Fact]
    public async Task be_invoked_when_the_test_server_is_created()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
        ServerFixtureInitializer._initializer.Verify(a => a.Initialize(It.IsAny<IServiceProvider>()));
    }
}
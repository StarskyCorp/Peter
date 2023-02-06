using System;
using System.Threading.Tasks;
using FluentAssertions;
using GreetingsApi;
using Moq;
using Peter.MinimalApi.Testing;
using Xunit;

namespace Peter.MinimalApi.Tests;

public class Initializer : IServerFixtureInitializer
{
    public static Mock<IServerFixtureInitializer> _initializer;

    static Initializer()
    {
        _initializer = new Mock<IServerFixtureInitializer>();
    }

    public void Initialize(IServiceProvider services)
    {
        _initializer.Object.Initialize(services);
    }
}

public class ServerFixtureInitializerShould : IClassFixture<ServerFixture<IApiMarker, Initializer>>
{
    private readonly ServerFixture<IApiMarker, Initializer> _fixture;

    public ServerFixtureInitializerShould(ServerFixture<IApiMarker, Initializer> fixture) => _fixture = fixture;

    [Fact]
    public async Task be_invoked_when_the_test_server_is_created()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
        Initializer._initializer.Verify(a => a.Initialize(It.IsAny<IServiceProvider>()));
    }
}
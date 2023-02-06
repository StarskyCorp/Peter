using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Peter.Testing;
using Xunit;

namespace Peter.Tests;

public class Initializer : IPeterInitializer
{
    public static Mock<IPeterInitializer> _initializer;

    static Initializer()
    {
        _initializer = new Mock<IPeterInitializer>();
    }

    public void Initialize(IServiceProvider services)
    {
        _initializer.Object.Initialize(services);
    }
}

public class InitializerShould : IClassFixture<ServerFixture<Program, Initializer>>
{
    private readonly ServerFixture<Program, Initializer> _fixture;

    public InitializerShould(ServerFixture<Program, Initializer> fixture) => _fixture = fixture;

    [Fact]
    public async Task be_invoked_when_the_test_server_is_created()
    {
        var response = await _fixture.Client().GetStringAsync("/Peter");
        response.Should().Be("Hello Peter!");
        Initializer._initializer.Verify(a => a.Initialize(It.IsAny<IServiceProvider>()));
    }
}
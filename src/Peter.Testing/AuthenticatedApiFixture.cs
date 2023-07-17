using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Peter.Testing;

public class AuthenticatedApiFixture<T> : WebApplicationFactory<T> where T : class
{
    public HttpClient CreateAuthenticatedClient(IEnumerable<Claim> claims) =>
        CreateDefaultClient().WithIdentity(claims);

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services => { services.AddTestAuthentication(); });

    private readonly object _lockObj = new();
    private bool _loggerProviderRegistered;

    public void RegisterLoggerProvider(ITestOutputHelper output)
    {
        lock (_lockObj)
        {
            if (_loggerProviderRegistered)
            {
                return;
            }

            var loggerFactory = base.Services.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddProvider(new XUnitLoggerProvider(output));
            _loggerProviderRegistered = true;
        }
    }
}

public class AuthenticatedApiFixture<T, Q> : AuthenticatedApiFixture<T>
    where T : class where Q : IAuthenticatedApiFixtureInitializer, new()
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var initializer = new Q();
            initializer.Initialize(scope.ServiceProvider);
        });
    }
}

public interface IAuthenticatedApiFixtureInitializer
{
    void Initialize(IServiceProvider services);
}
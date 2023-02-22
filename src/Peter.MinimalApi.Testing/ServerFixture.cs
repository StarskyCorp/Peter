﻿using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Peter.MinimalApi.Testing;

public class ServerFixture<T> : WebApplicationFactory<T> where T : class
{
    public HttpClient Client() => CreateDefaultClient();
    public HttpClient AuthenticatedClient(IEnumerable<Claim> claims) => CreateDefaultClient().WithIdentity(claims);

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
        {
            services.AddTestAuthentication();
        });

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

public class ServerFixture<T, Q> : ServerFixture<T> where T : class where Q : IServerFixtureInitializer, new()
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            using IServiceScope scope = services.BuildServiceProvider().CreateScope();
            var initializer = new Q();
            initializer.Initialize(scope.ServiceProvider);
        });
    }
}
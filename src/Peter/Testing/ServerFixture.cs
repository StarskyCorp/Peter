using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Peter.Testing;

public class ServerFixture<T> : WebApplicationFactory<T> where T : class
{
    public HttpClient Client() => CreateDefaultClient();
    public HttpClient AuthenticatedClient(IEnumerable<Claim> claims) => CreateDefaultClient().WithIdentity(claims);

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
        {
            services.AddTestAuthentication();
        });
}
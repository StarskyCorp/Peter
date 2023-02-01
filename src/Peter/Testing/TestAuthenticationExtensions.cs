using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Peter.Testing;

public static class TestAuthenticationExtensions
{
    public static AuthenticationBuilder AddTestAuthentication(this IServiceCollection services)
    {
        return services.AddAuthentication(defaultScheme: TestConstants.Authentication.TestScheme)
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                TestConstants.Authentication.TestScheme, options => { });
    }

    public static HttpClient WithIdentity(this HttpClient httpClient, IEnumerable<Claim> claims)
    {
        var serializedClaims = ClaimsSerializer.Encode(claims);

        httpClient.DefaultRequestHeaders.Add(
            name: TestConstants.Authentication.HeaderName,
            value: $"{TestConstants.Authentication.TestScheme} {serializedClaims}");

        return httpClient;
    }
}
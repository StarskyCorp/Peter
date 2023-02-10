using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Peter.MinimalApi.Testing;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class TestAuthenticationExtensions
{
    public static AuthenticationBuilder AddTestAuthentication(this IServiceCollection services)
    {
        return services.AddAuthentication(defaultScheme: TestConstants.Authentication.TestScheme)
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                TestConstants.Authentication.TestScheme, _ => { });
    }

    public static HttpClient WithIdentity(this HttpClient httpClient, IEnumerable<Claim> claims)
    {
        var serializedClaims = ClaimsSerializer.Serialize(claims);

        httpClient.DefaultRequestHeaders.Add(
            name: TestConstants.Authentication.HeaderName,
            value: $"{TestConstants.Authentication.TestScheme} {serializedClaims}");

        return httpClient;
    }
}
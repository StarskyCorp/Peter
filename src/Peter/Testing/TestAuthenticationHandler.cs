using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Peter.Testing;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string authorizationHeaderValue = Context.Request.Headers[TestConstants.Authentication.HeaderName];

        if (string.IsNullOrEmpty(authorizationHeaderValue))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!authorizationHeaderValue.StartsWith($"{Scheme.Name} ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var serializedClaims = authorizationHeaderValue.Substring($"{Scheme.Name} ".Length).Trim();
        var claims = ClaimsSerializer.Decode(serializedClaims);

        Logger.LogInformation("{Scheme} Authenticated", Scheme.Name);

        var identity = new ClaimsIdentity(claims, TestConstants.Authentication.TestAuthType);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), TestConstants.Authentication.TestScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
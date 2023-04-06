using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Peter.Testing;

internal static class ClaimsSerializer
{
    public static string Serialize(IEnumerable<Claim> claims)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var ticket = new AuthenticationTicket(principal, TestConstants.Authentication.TestScheme);
        var serializer = new TicketSerializer();
        var bytes = serializer.Serialize(ticket);
        return Convert.ToBase64String(bytes);
    }

    public static IEnumerable<Claim> Deserialize(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return Enumerable.Empty<Claim>();
        }

        var serializer = new TicketSerializer();
        var ticket = serializer.Deserialize(Convert.FromBase64String(text))!;
        return ticket.Principal.Claims;
    }
}
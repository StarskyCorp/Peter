using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Peter.Testing;

internal static class ClaimsSerializer
{
    public static string Encode(IEnumerable<Claim> claims)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var ticket = new AuthenticationTicket(principal,TestConstants.Authentication.TestScheme);
        var serializer = new TicketSerializer();
        var bytes = serializer.Serialize(ticket);
        return Convert.ToBase64String(bytes);
    }

    public static IEnumerable<Claim> Decode(string encodedValue)
    {
        if (string.IsNullOrEmpty(encodedValue))
        {
            return Enumerable.Empty<Claim>();
        }

        var serializer = new TicketSerializer();
        try
        {
            AuthenticationTicket ticket = serializer.Deserialize(Convert.FromBase64String(encodedValue));

            return ticket?.Principal.Claims;
        }
        catch (Exception)
        {
            return Enumerable.Empty<Claim>();
        }
    }
}
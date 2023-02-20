namespace Api.Tests.Features.Authentication;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder AddAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{who}", (string who) => $"Hello {who}!");
        app.MapGet("/{who}/authenticated",
                (string who, HttpContext context) =>
                    $"Hello {who}! These are your claims: {string.Join(",", context.User.Claims)}")
            .RequireAuthorization();

        return app;
    }
}
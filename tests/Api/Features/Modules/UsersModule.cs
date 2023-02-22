using Peter.MinimalApi.Modules;

namespace Api.Tests.Features.Modules;

public class UsersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroupBuilder = app.MapGroup("/users").WithTags("Users");
        routeGroupBuilder.MapGet("/", () => "Users");
        routeGroupBuilder.MapGet("/log", (ILogger<UsersModule> logger) =>
        {
            logger.LogInformation("Log inside endpoint");
            return "Users";
        });
        return app;
    }
}
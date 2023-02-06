using Peter.MinimalApi.Modules;

namespace GreetingsApi.Features.Modules;

public class UsersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/users").WithTags("Users");
        routeGroupBuilder.MapGet("/", () => "Users");
        return app;
    }
}
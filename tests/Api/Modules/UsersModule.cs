using Peter.MinimalApi.Modules;

namespace Api.Modules;

public class UsersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");
        group.MapGet("/", () => "Users");
        group.MapGet("/log", (ILogger<UsersModule> logger) =>
        {
            logger.LogInformation("Log inside endpoint");
            return "Users";
        });
        return app;
    }
}
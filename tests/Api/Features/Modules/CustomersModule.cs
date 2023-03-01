using Peter.MinimalApi.Modules;

namespace Api.Features.Modules;

public class CustomersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/customers").WithTags("Customers");
        routeGroupBuilder.MapGet("/", () => "Customers");
        return app;
    }
}
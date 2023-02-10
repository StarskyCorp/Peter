using Peter.MinimalApi.Modules;

namespace Api.Tests.Features.Modules;

public class CustomersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/customers").WithTags("Customers");
        routeGroupBuilder.MapGet("/", () => "Customers");
        return app;
    }
}
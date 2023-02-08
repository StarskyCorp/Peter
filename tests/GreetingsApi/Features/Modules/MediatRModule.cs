using GreetingsApi.Features.Commands;
using GreetingsApi.Features.Queries;
using GreetingsApi.Features.Validation;
using Peter.MinimalApi.Modules;

namespace GreetingsApi.Features.Modules;

public class MediatRModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/MediatRGroupProducts").WithTags("MediatRGroupProducts");

        routeGroupBuilder.MapGetMediatR<ProductsQuery, IEnumerable<Product>>("/");
        routeGroupBuilder.MapPostMediatR<AddProductCommand, Product>("/");
        routeGroupBuilder.MapPutMediatR<UpdateProductCommand, Product>("/");
        routeGroupBuilder.MapPatchMediatR<UpdateProductCommand, Product>("/");
        return app;
    }
}
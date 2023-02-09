using Api.Tests.Features.Commands;
using Api.Tests.Features.Queries;
using Api.Tests.Features.Validation;
using Peter.MinimalApi.Modules;

namespace Api.Tests.Features.Modules;

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
using Api.Tests.Features.Commands;
using Api.Tests.Features.Queries;
using Api.Tests.Features.Validation;
using MediatR;
using Peter.MinimalApi.Modules;

namespace Api.Tests.Features.Modules;

public class MediatRModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/MediatRGroupProducts").WithTags("MediatRGroupProducts");

        routeGroupBuilder.MapGet<ProductsQuery, IEnumerable<Product>>("/");
        routeGroupBuilder.MapPost<AddProductCommand, Product>("/");
        routeGroupBuilder.MapPut<UpdateProductCommand, Product>("/");
        routeGroupBuilder.MapPatch<UpdateProductCommand, Product>("/");
        routeGroupBuilder.MapDelete<DeleteProductCommand, Unit>("/{id}");
        return app;
    }
}
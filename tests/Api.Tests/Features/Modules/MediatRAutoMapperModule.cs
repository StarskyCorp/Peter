using Api.Tests.Features.Commands;
using Api.Tests.Features.Queries;
using Api.Tests.Features.Validation;
using MediatR;
using Peter.MinimalApi.Modules;

namespace Api.Tests.Features.Modules;

public class MediatRAutoMapperModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/MediatRAutoMapperGroupProducts").WithTags("MediatRAutoMapperGroupProducts");

        routeGroupBuilder.MapGet<ProductsDto, ProductsQuery, IEnumerable<Product>>("/");
        routeGroupBuilder.MapPost<AddProductDto, AddProductCommand, Product>("/");
        routeGroupBuilder.MapPut<UpdateProductProductDto, UpdateProductCommand, Product>("/");
        routeGroupBuilder.MapPatch<UpdateProductProductDto, UpdateProductCommand, Product>("/");
        routeGroupBuilder.MapDelete<DeleteProductDto, DeleteProductCommand, Unit>("/{id}");
        return app;
    }
}
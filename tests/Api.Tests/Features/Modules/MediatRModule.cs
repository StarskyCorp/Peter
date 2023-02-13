using Api.Tests.Features.Commands;
using Api.Tests.Features.Queries;
using Api.Tests.Features.Validation;
using AutoMapper;
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

public class ProductsDto
{
    public string? Name { get; set; }
}

public class AddProductDto
{
    public string Name { get; set; }
}

public class DeleteProductDto
{
    public int Id { get; set; }
}

public class UpdateProductProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ProductsDtoProfile : Profile
{
    public ProductsDtoProfile()
    {
        CreateMap<ProductsDto, ProductsQuery>();
        CreateMap<AddProductDto, AddProductCommand>();
        CreateMap<UpdateProductProductDto, UpdateProductCommand>();
        CreateMap<DeleteProductDto, DeleteProductCommand>();
    }
}
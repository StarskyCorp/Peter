using GreetingsApi.Features.Validation;
using MediatR;

namespace GreetingsApi.Features.Queries;

public class ProductsQuery : IRequest<IEnumerable<Product>>
{
    public string? Name { get; set; }
}

public class ProductsQueryHandler : IRequestHandler<ProductsQuery, IEnumerable<Product>>
{
    public Task<IEnumerable<Product>> Handle(ProductsQuery request, CancellationToken cancellationToken = default)
    {
        var products = new List<Product> { new() { Id = 1, Name = "P1" }, new() { Id = 2, Name = "P2" }, new() { Id = 3, Name = "P3" } };
        return Task.FromResult(products.Where(p => p.Name == request.Name).AsEnumerable());
    }
}
using Api.Tests.Features.Validation;
using MediatR;

namespace Api.Tests.Features.Commands;

public class AddProductCommand : IRequest<Product>
{
    public string? Name { get; set; }
}

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Product>
{
    public Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Product
        {
            Id = 99, //We assign 99 tto new product as convention for this test site
            Name = request.Name
        });
    }
}
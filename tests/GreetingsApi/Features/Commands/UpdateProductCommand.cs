using Api.Tests.Features.Validation;
using MediatR;

namespace Api.Tests.Features.Commands;

public class UpdateProductCommand : IRequest<Product>
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
{
    public Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Product
        {
            Id = request.Id,
            Name = request.Name
        });
    }
}
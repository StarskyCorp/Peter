using MediatR;

namespace Api.Tests.Features.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    public Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken = default)
    {
        if (request.Id != 5)
        {
            throw new ArgumentException(nameof(request.Id));
        }

        return Task.FromResult(new Unit());
    }
}
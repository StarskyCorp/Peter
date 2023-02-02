using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class MediatRExtensions
{
    public static WebApplication MapGetMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapGet(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send<TResponse>(request);
        });

        return app;
    }

    public static WebApplication MapPostMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPost(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send<TResponse>(request);
        });

        return app;
    }

    public static WebApplication MapPutMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPut(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send<TResponse>(request);
        });

        return app;
    }
}
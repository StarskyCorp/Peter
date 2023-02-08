using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

//ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class MediatRExtensions
{
    public static WebApplication MapGetMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapGet(pattern, async ([FromServices] IMediator mediator, [AsParameters] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapGetMediatR<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapGet(pattern, async ([FromServices] IMediator mediator, [AsParameters] TRequest request) =>
        {
            return await mediator.Send(request);
        });

        return app;
    }

    public static WebApplication MapPostMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPost(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPostMediatR<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPost(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static WebApplication MapPutMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPut(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPutMediatR<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPut(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static WebApplication MapPatchMediatR<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPatch(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPatchMediatR<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPatch(pattern, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }
}
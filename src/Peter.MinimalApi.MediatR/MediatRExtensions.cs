using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

//ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class MediatRExtensions
{
    public static IEndpointRouteBuilder MapGet<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapGet(pattern, async (IMediator mediator, [AsParameters] TRequest request) =>
        {
            return await mediator.Send(request);
        });

        return app;
    }

    public static IEndpointRouteBuilder MapPost<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPost(pattern, async (IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPut<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPut(pattern, async (IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPatch<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPatch(pattern, async (IMediator mediator, [FromBody] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }

    public static IEndpointRouteBuilder MapDelete<TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapDelete(pattern, async (IMediator mediator, [AsParameters] TRequest request) =>
        {
            return await mediator.Send(request);
        });
        return app;
    }
}
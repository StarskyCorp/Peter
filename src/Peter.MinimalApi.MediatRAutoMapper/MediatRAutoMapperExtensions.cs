using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

//ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class MediatRAutoMapperExtensions
{
    public static IEndpointRouteBuilder MapGet<TDto, TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapGet(pattern, async (IMapper mapper, IMediator mediator, [AsParameters] TDto dto) =>
        {
            return await mediator.Send(mapper.Map<TDto, TRequest>(dto));
        });

        return app;
    }

    public static IEndpointRouteBuilder MapPost<TDto, TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPost(pattern, async (IMapper mapper, IMediator mediator, [FromBody] TDto dto) =>
        {
            return await mediator.Send(mapper.Map<TDto, TRequest>(dto));
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPut<TDto, TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPut(pattern, async (IMapper mapper, IMediator mediator, [FromBody] TDto dto) =>
        {
            return await mediator.Send(mapper.Map<TDto, TRequest>(dto));
        });
        return app;
    }

    public static IEndpointRouteBuilder MapPatch<TDto, TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapPatch(pattern, async (IMapper mapper, IMediator mediator, [FromBody] TDto dto) =>
        {
            return await mediator.Send(mapper.Map<TDto, TRequest>(dto));
        });
        return app;
    }

    public static IEndpointRouteBuilder MapDelete<TDto, TRequest, TResponse>(this IEndpointRouteBuilder app, string pattern) where TRequest : IRequest<TResponse>
    {
        app.MapDelete(pattern, async (IMapper mapper, IMediator mediator, [AsParameters] TDto dto) =>
        {
            return await mediator.Send(mapper.Map<TDto, TRequest>(dto));
        });
        return app;
    }
}
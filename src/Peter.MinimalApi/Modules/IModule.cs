using Microsoft.AspNetCore.Routing;

namespace Peter.MinimalApi.Modules;

public interface IModule
{
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app);
}
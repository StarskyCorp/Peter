﻿using Peter.MinimalApi.Modules;

namespace Api.Features.Modules;

public class CustomersModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers").WithTags("Customers");
        group.MapGet("/", () => "Customers");
        return app;
    }
}
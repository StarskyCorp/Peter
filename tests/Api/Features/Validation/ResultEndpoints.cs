using Microsoft.AspNetCore.Mvc;
using Peter.Result;

namespace Api.Features.Validation;

public static class ResultEndpoints
{
    public static IEndpointRouteBuilder AddResultEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/ok", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi();
        });

        app.MapPost("/created", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseCreated("/any_url"));
        });

        app.MapPost("/created_at_route", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseCreatedAtRoute("GetFoo", new { id = 1 }));
        });

        app.MapPost("/accepted", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseAccepted("/any_url"));
        });

        app.MapPost("/accepted_at_route", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseAcceptedAtRoute("GetFoo", new { id = 2 }));
        });

        app.MapGet("/foo/{id:int}", (int id) => { }).WithName("GetFoo");

        app.MapGet("/problem", () =>
        {
            var result = ErrorResult<object>.Create(new[] { new Error("A failure") });
            return result.ToMinimalApi();
        });

        app.MapGet("/internal_server_error", () =>
        {
            var result = ErrorResult<object>.Create("A failure");
            return result.ToMinimalApi(options => { options.UseInternalServerError(); });
        });

        app.MapGet("/not_found", () =>
        {
            var result = NotFoundResult<object>.Create("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/no_content", () =>
        {
            var result = NotFoundResult<object>.Create();
            return result.ToMinimalApi(options => options.UseNoContent());
        });

        app.MapGet("/validation_problem", () =>
        {
            var result = InvalidResult<object>.Create("peter", "message");
            return result.ToMinimalApi();
        });

        app.MapGet("/bad_request", () =>
        {
            var result = InvalidResult<object>.Create("peter", "message");
            return result.ToMinimalApi(options => options.UseBadRequest());
        });

        return app;
    }
}
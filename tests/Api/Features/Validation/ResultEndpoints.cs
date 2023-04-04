using Microsoft.AspNetCore.Mvc;
using Peter.Result;
using Peter.Result.MinimalApi;

namespace Api.Features.Validation;

public static class ResultEndpoints
{
    public static IEndpointRouteBuilder AddResultEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/not_found", () =>
        {
            var result = NotExistResult<object>.Create();
            return result.ToMinimalApi();
        });

        app.MapGet("/not_found_with_value", () =>
        {
            var result = NotExistResult<object>.Create("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/no_content", () =>
        {
            var result = NotExistResult<object>.Create();
            return result.ToMinimalApi(options => options.WithNoContentBehaviour());
        });

        app.MapGet("/bad_request", () =>
        {
            var result = InvalidResult<object>.Create(new[] { new ValidationProblemError("peter", "message") });
            return result.ToMinimalApi();
        });

        app.MapGet("/internal_server_error_using_problem_details", () =>
        {
            var result = Result<object>.CreateFailure(new[] { new Error("A failure") });
            return result.ToMinimalApi();
        });

        app.MapGet("/internal_server_error_not_using_problem_details", () =>
        {
            var result = Result<object>.CreateFailure(new[] { new Error("A failure") });
            return result.ToMinimalApi(options => { options.UseProblemDetails = false; });
        });

        app.MapGet("/ok", () =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi();
        });

        app.MapPost("/created", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithCreatedBehaviour("/any_url"));
        });

        app.MapPost("/created_at", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithCreatedAtBehaviour("GetFoo", new { id = 1 }));
        });

        app.MapPost("/accepted", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithAcceptedBehaviour("/any_url"));
        });

        app.MapPost("/accepted_at", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithAcceptedAtBehaviour("GetFoo", new { id = 2 }));
        });

        app.MapGet("/foo/{id:int}", (int id) => { }).WithName("GetFoo");

        return app;
    }
}
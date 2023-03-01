using Microsoft.AspNetCore.Mvc;
using Peter.Result;

namespace Api.Features.Validation;

public static class ResultEndpoints
{
    public static IEndpointRouteBuilder AddResultEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/ok", () =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi();
        });

        app.MapGet("/foo/{id:int}", (int id) =>
        {
        }).WithName("GetFoo");

        app.MapPost("/created_at", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithCreatedAtBehaviour("GetFoo", new { id = 1 }));
        });

        app.MapPost("/created", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithCreatedBehaviour("/anyUrl"));
        });

        app.MapPost("/accepted_at", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithAcceptedAtBehaviour("GetFoo", new { id = 2 }));
        });

        app.MapPost("/accepted", ([FromBody] string payload) =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithAcceptedBehaviour("/anyUrl"));
        });

        app.MapGet("/failed_using_problem_details", () =>
        {
            var result = Result<object>.CreateFailure(new[] { "A failure" });
            return result.ToMinimalApi();
        });

        app.MapGet("/failed_not_using_problem_details", () =>
        {
            var result = Result<object>.CreateFailure(new[] { "A failure" });
            return result.ToMinimalApi(options =>
            {
                options.UseProblemDetails = false;
            });
        });

        app.MapGet("/not_exists", () =>
        {
            var result = NotExistsResult<object>.Create();
            return result.ToMinimalApi();
        });

        app.MapGet("/no_content", () =>
        {
            var result = NotExistsResult<object>.Create();
            return result.ToMinimalApi(options => options.WithNoContentBehaviour());
        });

        app.MapGet("/not_exists_with_value", () =>
        {
            var result = NotExistsResult<object>.Create("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/invalid", () =>
        {
            var result = InvalidResult<object>.Create(new[] { new ValidationError("peter", "message") });
            return result.ToMinimalApi();
        });

        return app;
    }
}
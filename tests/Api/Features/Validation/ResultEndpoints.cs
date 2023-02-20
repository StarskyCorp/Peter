﻿using Peter.Result;

namespace Api.Tests.Features.Validation;

public static class ResultEndpoints
{
    public static IEndpointRouteBuilder AddResultEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/ok", () =>
        {
            Result<string> result = "Peter";
            return result.ToMinimalApi();
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
            var result = Result<object>.CreateNotExists();
            return result.ToMinimalApi();
        });

        app.MapGet("/not_exists_with_value", () =>
        {
            var result = Result<object>.CreateNotExists("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/invalid", () =>
        {
            var result = Result<object>.CreateInvalid(new[] { new ValidationError("peter", "message") });
            return result.ToMinimalApi();
        });

        return app;
    }
}
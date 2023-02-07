﻿using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Peter.Result;

public static class ResultExtensions
{
    public static IResult ToMinimalApi<T>(this Result<T> result, Action<ToMinimalApiOptions> configure)
    {
        var options = new ToMinimalApiOptions();
        configure(options);

        return result.Status switch
        {
            ResultStatus.Success => Results.Ok(result.Value),
            ResultStatus.Failure => ManageProblem(result, options),
            ResultStatus.NotExists => Results.NotFound(result.Value),
            ResultStatus.Invalid => ManageInvalid(result, options),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageProblem<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (options.UseProblemDetails && result.Errors != null)
        {
            return Results.Problem(detail: string.Join(",", result.Errors),
                title: "Error",
                statusCode: StatusCodes.Status500InternalServerError);
        }
        return Results.StatusCode(500);
    }

    private static IResult ManageInvalid<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (options.UseProblemDetails)
        {
            return Results.ValidationProblem(result.ToProblemDetails());
        }

        return Results.BadRequest(result.ValidationErrors);
    }

    public static IDictionary<string, string[]> ToProblemDetails<T>(this Result<T> result)
    {
        var details = result.ValidationErrors?
            .GroupBy(x => x.Field)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Message).ToArray());

        return details ?? new Dictionary<string, string[]>();
    }
}
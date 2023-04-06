using Microsoft.AspNetCore.Http;
using Peter.Result.MinimalApi;

// ReSharper disable once CheckNamespace
namespace Peter.Result;

public static class ResultExtensions
{
    public static IResult ToMinimalApi<T>(this Result<T> result, Action<ToMinimalApiOptions> configure)
    {
        var customHandler = ToMinimalApiOptions.GetCustomHandler(result.GetType());
        if (customHandler is not null)
        {
            return customHandler(result);
        }

        var options = ToMinimalApiOptions.Create();
        configure(options);

        return result switch
        {
            OkResult<T> => ManageOk(result, options),
            ErrorResult<T> errorResult => ManageError(errorResult, options),
            NotFoundResult<T> => ManageNotFound(result, options),
            InvalidResult<T> invalidResult => ManageInvalid(invalidResult, options),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result.GetType(),
                $"{nameof(ToMinimalApi)} is not able to handle type {result.GetType()}")
        };
    }

    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageOk<T>(Result<T> result, ToMinimalApiOptions options)
    {
        return options.Ok switch
        {
            OkType.Ok => Results.Ok(result.Value),
            OkType.Created or OkType.CreatedAtRoute => ManageCreated(result, options),
            OkType.Accepted or OkType.AcceptedAtRoute => ManageAccepted(result, options),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result.GetType(),
                $"{nameof(ManageOk)} is not able to handle type {result.GetType()}")
        };
    }

    private static IResult ManageCreated<T>(Result<T> result, ToMinimalApiOptions options)
    {
        return options.Ok is OkType.Created
            ? Results.Created(options.Uri!, result.Value)
            : Results.CreatedAtRoute(options.RouteName, options.RouteValues, result.Value);
    }

    private static IResult ManageAccepted<T>(Result<T> result, ToMinimalApiOptions options)
    {
        return options.Ok is OkType.Accepted
            ? Results.Accepted(options.Uri, result.Value)
            : Results.AcceptedAtRoute(options.RouteName, options.RouteValues, result.Value);
    }

    private static IResult ManageError<T>(ErrorResult<T> result, ToMinimalApiOptions options)
    {
        if (options.Error is ErrorType.Problem)
        {
            return Results.Problem(detail: string.Join(",", result.Errors.Select(e => e.Message)),
                title: "Error",
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }

    private static IResult ManageNotFound<T>(Result<T> result, ToMinimalApiOptions options)
    {
        return options.NotFound is NotFoundType.NotFound
            ? Results.NotFound((object?)result.Value)
            : Results.NoContent();
    }

    private static IResult ManageInvalid<T>(InvalidResult<T> result, ToMinimalApiOptions options)
    {
        return options.Invalid is InvalidType.ValidationProblem
            ? Results.ValidationProblem(result.ToProblemDetails())
            : Results.BadRequest(result.ValidationErrors);
    }

    private static IDictionary<string, string[]> ToProblemDetails<T>(this InvalidResult<T> result)
    {
        return result.ValidationErrors
            .GroupBy(x => x.Identifier)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Message).ToArray());
    }
}
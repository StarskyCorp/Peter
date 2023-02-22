using Microsoft.AspNetCore.Http;
using Peter.Result.MinimalApi;

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
            ResultStatus.Success => ManageOk(result),
            ResultStatus.Created => ManageCreated(result),
            ResultStatus.Failure => ManageFailure(result, options),
            ResultStatus.NotExists => Results.NotFound(result.Value),
            ResultStatus.Invalid => ManageInvalid(result, options),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    private static IResult ManageOk<T>(Result<T> result) => Results.Ok(result.Value);

    private static IResult ManageCreated<T>(Result<T> result)
    {
        if (result.RouteInfo!.RouteValues is null)
        {
            return Results.Created(result.RouteInfo.Route!, result.Value);
        }
        return Results.CreatedAtRoute(result.RouteInfo.Route, result.RouteInfo.RouteValues, result.Value);
    }

    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageFailure<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (options.UseProblemDetails && result.Errors != null)
        {
            return Results.Problem(detail: string.Join(",", result.Errors),
                title: "Error",
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return Results.StatusCode(500);
    }

    private static IResult ManageInvalid<T>(Result<T> result, ToMinimalApiOptions options) => options.UseProblemDetails
        ? Results.ValidationProblem(result.ToProblemDetails())
        : Results.BadRequest(result.ValidationErrors);

    private static IDictionary<string, string[]> ToProblemDetails<T>(this Result<T> result)
    {
        var details = result.ValidationErrors?
            .GroupBy(x => x.Field)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Message).ToArray());

        return details ?? new Dictionary<string, string[]>();
    }
}
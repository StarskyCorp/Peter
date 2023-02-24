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
            ResultStatus.Success => ManageOk(result, options),
            ResultStatus.Failure => ManageFailure(result, options),
            ResultStatus.NotExists => ManageNotExists(result, options),
            ResultStatus.Invalid => ManageInvalid(result, options),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageNotExists<T>(Result<T> result, ToMinimalApiOptions options)
        => options.NoContentBehaviour is NoContentBehaviourType.NotFound ? Results.NotFound(result.Value) : Results.NoContent();

    private static IResult ManageOk<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (options.OkBehaviour is OkBehaviourType.Created or OkBehaviourType.CreatedAt)
        {
            return ManageCreated(result, options);
        }

        if (options.OkBehaviour is OkBehaviourType.Accepted or OkBehaviourType.AcceptedAt)
        {
            return ManageAccepted(result, options);
        }
        return Results.Ok(result.Value);
    }

    private static IResult ManageAccepted<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Route))
        {
            throw new ArgumentNullException(nameof(options.Route));
        }

        if (options.OkBehaviour == OkBehaviourType.Accepted)
        {
            return Results.Accepted(options.Route, result.Value);
        }

        return Results.AcceptedAtRoute(options.Route, options.RouteValues, result.Value);
    }

    private static IResult ManageCreated<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Route))
        {
            throw new ArgumentNullException(nameof(options.Route));
        }

        if (options.OkBehaviour == OkBehaviourType.Created)
        {
            return Results.Created(options.Route, result.Value);
        }

        return Results.CreatedAtRoute(options.Route, options.RouteValues, result.Value);
    }

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
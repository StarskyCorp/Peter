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

        return result.GetType() switch
        {
            var type when type == typeof(NotExistResult<T>) => ManageNotExist(result, options),
            var type when type == typeof(InvalidResult<T>) => ManageInvalid((InvalidResult<T>)result, options),
            var type when type == typeof(Result<T>) => result.Ok switch
            {
                true => ManageOk(result, options),
                false => ManageFailure(result, options)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageNotExist<T>(Result<T> result, ToMinimalApiOptions options)
        => options.NoContentBehaviour is NoContentBehaviourType.NotFound
            ? Results.NotFound(result.Value)
            : Results.NoContent();

    private static IResult ManageInvalid<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.UseProblemDetails
            ? Results.ValidationProblem(result.ToProblemDetails())
            : Results.BadRequest(result.Errors);

    private static IResult ManageOk<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.OkBehaviour switch
        {
            OkBehaviourType.Created or OkBehaviourType.CreatedAt => ManageCreated(result, options),
            OkBehaviourType.Accepted or OkBehaviourType.AcceptedAt => ManageAccepted(result, options),
            _ => Results.Ok(result.Value)
        };

    private static IResult ManageCreated<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Route))
        {
            throw new ArgumentNullException(nameof(options.Route));
        }

        return options.OkBehaviour == OkBehaviourType.Created
            ? Results.Created(options.Route, result.Value)
            : Results.CreatedAtRoute(options.Route, options.RouteValues, result.Value);
    }

    private static IResult ManageAccepted<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Route))
        {
            throw new ArgumentNullException(nameof(options.Route));
        }

        return options.OkBehaviour == OkBehaviourType.Accepted
            ? Results.Accepted(options.Route, result.Value)
            : Results.AcceptedAtRoute(options.Route, options.RouteValues, result.Value);
    }

    private static IResult ManageFailure<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (options.UseProblemDetails && result.Errors != null)
        {
            return Results.Problem(detail: string.Join(",", result.Errors.Select(e => e.Message)),
                title: "Error",
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }

    private static IDictionary<string, string[]> ToProblemDetails<T>(this Result<T> result)
    {
        var problemDetails = result.Errors?.Cast<ValidationProblemError>()
            .GroupBy(x => x.Identifier)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Message).ToArray());

        return problemDetails ?? new Dictionary<string, string[]>();
    }
}
using Microsoft.AspNetCore.Http;
using Peter.Result.MinimalApi;

// ReSharper disable once CheckNamespace
namespace Peter.Result;

public static class ResultExtensions
{
    public static IResult ToMinimalApi<T>(this Result<T> result, Action<ToMinimalApiOptions> configure)
    {
        var type = result.GetType();
        Func<object, IResult>? customHandler;
        if (type.IsGenericType)
        {
            customHandler = ToMinimalApiOptions.GetCustomHandler(type);
            if (customHandler is not null)
            {
                return customHandler(result);
            }

            var genericType = type.GetGenericTypeDefinition();
            customHandler = ToMinimalApiOptions.GetCustomHandler(genericType);
            if (customHandler is not null)
            {
                return customHandler(result);
            }
        }
        else
        {
            customHandler = ToMinimalApiOptions.GetCustomHandler(type);
            if (customHandler is not null)
            {
                return customHandler(result);
            }
        }

        var options = ToMinimalApiOptions.Create();
        configure(options);

        return result switch
        {
            OkResult<T> => ManageOk(result, options),
            ErrorResult<T> errorResult => ManageError(errorResult, options),
            NotFoundResult<T> => ManageNotFound(result, options),
            InvalidResult<T> invalidResult => ManageInvalid(invalidResult, options),
            _ => result.Ok ? ManageOk(result, options) : ManageError(result, options)
        };
    }

    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageOk<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (result is not OkResult<T> okResult)
        {
            return Results.Ok(GetValue(result));
        }

        return options.Ok switch
        {
            OkType.Ok => Results.Ok(GetValue(okResult)),
            OkType.Created or OkType.CreatedAtRoute => ManageCreated(okResult, options),
            OkType.Accepted or OkType.AcceptedAtRoute => ManageAccepted(okResult, options),
            _ => throw new ArgumentOutOfRangeException(nameof(okResult), okResult.GetType(),
                $"{nameof(ManageOk)} is not able to handle type {okResult.GetType()}")
        };
    }

    private static IResult ManageCreated<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.Ok is OkType.Created
            ? Results.Created(options.Uri!, GetValue(result))
            : Results.CreatedAtRoute(options.RouteName, options.RouteValues, GetValue(result));

    private static IResult ManageAccepted<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.Ok is OkType.Accepted
            ? Results.Accepted(options.Uri, GetValue(result))
            : Results.AcceptedAtRoute(options.RouteName, options.RouteValues, GetValue(result));

    private static IResult ManageError<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (result is ErrorResult<T> errorResult)
        {
            var content = string.Join(",", errorResult.Errors.Select(e => e.Message));
            if (options.Error is ErrorType.Problem)
            {
                return Results.Problem(detail: content,
                    title: "Error",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            return new InternalServerErrorResult(content);
        }

        return !options.InternalServerErrorToString
            ? Results.StatusCode(StatusCodes.Status500InternalServerError)
            : new InternalServerErrorResult(result.ToString()!);
    }

    private static IResult ManageNotFound<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.NotFound is NotFoundType.NotFound
            ? Results.NotFound(GetValue(result))
            : Results.NoContent();

    private static IResult ManageInvalid<T>(InvalidResult<T> result, ToMinimalApiOptions options)
    {
        if (options.Invalid is InvalidType.ValidationProblem)
        {
            return Results.ValidationProblem(result.ToProblemDetails());
        }

        return !options.SimpleBadRequest
            ? Results.BadRequest(result.ValidationErrors)
            : Results.BadRequest(result.ValidationErrors.Select(ve => ve.Message));
    }

    private static IDictionary<string, string[]> ToProblemDetails<T>(this InvalidResult<T> result) =>
        result.ValidationErrors
            .GroupBy(x => x.Identifier)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Message).ToArray());

    private static object? GetValue<T>(Result<T> result)
    {
        var type = result.Value?.GetType();
        if (type is not null && ToMinimalApiOptions.GetNullTypes().Contains(type))
        {
            return null;
        }

        return result.Value;
    }
}
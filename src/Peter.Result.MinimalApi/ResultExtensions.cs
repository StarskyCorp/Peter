﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peter.Result.MinimalApi;

// ReSharper disable once CheckNamespace
namespace Peter.Result;

public static class ResultExtensions
{
    public static IResult ToMinimalApi<T>(this Result<T> result, Action<ToMinimalApiOptions> configure)
    {
        var options = ToMinimalApiOptions.Create();
        configure(options);

        var type = result.GetType();
        Func<object, IResult>? customHandler;

        if (type.IsGenericType)
        {
            customHandler = options.Configuration.GetCustomHandler(type);
            if (customHandler is not null)
            {
                return customHandler(result);
            }

            var genericType = type.GetGenericTypeDefinition();
            customHandler = options.Configuration.GetCustomHandler(genericType);
            if (customHandler is not null)
            {
                return customHandler(result);
            }
        }
        else
        {
            customHandler = options.Configuration.GetCustomHandler(type);
            if (customHandler is not null)
            {
                return customHandler(result);
            }
        }

        return result switch
        {
            OkResult<T> => ManageOk(result, options),
            ErrorResult<T> errorResult => ManageError(errorResult, options),
            NotFoundResult<T> => ManageNotFound(),
            InvalidResult<T> invalidResult => ManageInvalid(invalidResult, options),
            NotAllowedResult<T> => ManageNotAllowed(),
            _ => result.Ok ? ManageOk(result, options) : ManageError(result, options)
        };
    }


    public static IResult ToMinimalApi<T>(this Result<T> result) => result.ToMinimalApi(_ => { });

    private static IResult ManageOk<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.Ok switch
        {
            OkType.Ok => Results.Ok(GetValue(result, options)),
            OkType.Created or OkType.CreatedAtRoute => ManageCreated(result, options),
            OkType.Accepted or OkType.AcceptedAtRoute => ManageAccepted(result, options),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static IResult ManageCreated<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.Ok is OkType.Created
            ? Results.Created(options.Uri!, GetValue(result, options))
            : Results.CreatedAtRoute(options.RouteName, options.RouteValues, GetValue(result, options));

    private static IResult ManageAccepted<T>(Result<T> result, ToMinimalApiOptions options) =>
        options.Ok is OkType.Accepted
            ? Results.Accepted(options.Uri, GetValue(result, options))
            : Results.AcceptedAtRoute(options.RouteName, options.RouteValues, GetValue(result, options));

    private static IResult ManageError<T>(Result<T> result, ToMinimalApiOptions options)
    {
        if (result is not ErrorResult<T> errorResult)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (options.Error is ErrorType.Problem)
        {
            return Results.Problem(new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Detail = errorResult.Error
            });
        }

        return new InternalServerErrorResult(errorResult.Error);
    }


    private static IResult ManageNotFound() => Results.NotFound();

    private static IResult ManageInvalid<T>(InvalidResult<T> result, ToMinimalApiOptions options) =>
        result switch
        {
            SimpleInvalidResult<T> simpleInvalidResult when options.Invalid is InvalidType.BadRequest =>
                Results.BadRequest(simpleInvalidResult.Message),
            SimpleInvalidResult<T> simpleInvalidResult => Results.Problem(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "An error occurred while processing your request.",
                Detail = simpleInvalidResult.Message,
            }),
            DetailedInvalidResult<T> detailedInvalidResult when options.Invalid is InvalidType.BadRequest =>
                Results.BadRequest(detailedInvalidResult.Details),
            DetailedInvalidResult<T> detailedInvalidResult => Results.ValidationProblem(detailedInvalidResult
                .ToProblemDetails()),
            _ => Results.BadRequest()
        };

    private static IResult ManageNotAllowed() => Results.Forbid();

    private static IDictionary<string, string[]> ToProblemDetails<T>(this DetailedInvalidResult<T> result) =>
        result.Details
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.SelectMany(e => e.Messages).ToArray());

    private static object? GetValue<T>(Result<T> result, ToMinimalApiOptions toMinimalApiOptions)
    {
        var type = result.Value?.GetType();
        if (toMinimalApiOptions.Configuration.ExistsNullType(type))
        {
            return null;
        }

        return result.Value;
    }
}
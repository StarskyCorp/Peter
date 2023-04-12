using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace Peter.Result.MinimalApi;

public class ToMinimalApiOptions : ICloneable
{
    public OkType Ok { get; private set; }

    public ErrorType Error { get; private set; }

    public NotFoundType NotFound { get; private set; }

    public InvalidType Invalid { get; private set; }

    public string? Uri { get; private set; }

    public string? RouteName { get; private set; }

    public object? RouteValues { get; private set; }

    public bool SimpleBadRequest { get; private set; }

    public bool InternalServerErrorToString { get; private set; }

    private static readonly ToMinimalApiOptions DefaultOptions = new();

    private static readonly ConcurrentDictionary<Type, Func<object, IResult>> CustomHandlers = new();

    private ToMinimalApiOptions()
    {
        Ok = OkType.Ok;
        Error = ErrorType.Problem;
        NotFound = NotFoundType.NotFound;
        Invalid = InvalidType.ValidationProblem;
        SimpleBadRequest = false;
        InternalServerErrorToString = false;
    }

    public static ToMinimalApiOptions GetDefaultOptions()
    {
        return DefaultOptions;
    }

    public static ToMinimalApiOptions Create()
    {
        return (ToMinimalApiOptions)DefaultOptions.Clone();
    }

    public static void UseCustomHandler(Type type, Func<object, IResult> handler)
    {
        CustomHandlers[type] = handler;
    }

    public static Func<object, IResult>? GetCustomHandler(Type type) =>
        CustomHandlers.TryGetValue(type, out var handler) ? handler : null;

    public void UseOk()
    {
        Ok = OkType.Ok;
    }

    public void UseCreated(string? uri)
    {
        Ok = OkType.Created;
        Uri = uri;
    }

    public void UseCreatedAtRoute(string? routeName, object? routeValues = null)
    {
        Ok = OkType.CreatedAtRoute;
        RouteName = routeName;
        RouteValues = routeValues;
    }

    public void UseAccepted(string? uri)
    {
        Ok = OkType.Accepted;
        Uri = uri;
    }

    public void UseAcceptedAtRoute(string? routeName, object? routeValues = null)
    {
        Ok = OkType.AcceptedAtRoute;
        RouteName = routeName;
        RouteValues = routeValues;
    }

    public void UseProblem()
    {
        Error = ErrorType.Problem;
    }

    public void UseInternalServerError(bool toString = false)
    {
        Error = ErrorType.InternalServerError;
        InternalServerErrorToString = toString;
    }

    public void UseNotFound()
    {
        NotFound = NotFoundType.NotFound;
    }

    public void UseNoContent()
    {
        NotFound = NotFoundType.NoContent;
    }

    public void UseValidationProblem()
    {
        Invalid = InvalidType.ValidationProblem;
    }

    public void UseBadRequest(bool simple = false)
    {
        Invalid = InvalidType.BadRequest;
        SimpleBadRequest = simple;
    }

    public object Clone() =>
        new ToMinimalApiOptions
        {
            Ok = Ok,
            Error = Error,
            NotFound = NotFound,
            Invalid = Invalid
        };
}
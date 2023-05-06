using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace Peter.Result.MinimalApi;

public class ToMinimalApiOptions : ICloneable
{
    public OkType Ok { get; private set; }

    public ErrorType Error { get; private set; }

    public InvalidType Invalid { get; private set; }

    public string? Uri { get; private set; }

    public string? RouteName { get; private set; }

    public object? RouteValues { get; private set; }

    private static readonly List<Type> NullTypes = new() { typeof(Void) };

    private static readonly ToMinimalApiOptions DefaultOptions = new();

    private static readonly ConcurrentDictionary<Type, Func<object, IResult>> CustomHandlers = new();

    private ToMinimalApiOptions()
    {
        Ok = OkType.Ok;
        Error = ErrorType.Problem;
        Invalid = InvalidType.Problem;
    }

    public static ToMinimalApiOptions GetDefaultOptions()
    {
        return DefaultOptions;
    }

    public static ToMinimalApiOptions Create()
    {
        return (ToMinimalApiOptions)DefaultOptions.Clone();
    }

    public static void AddNullType(Type type)
    {
        NullTypes.Add(type);
    }

    public static IEnumerable<Type> GetNullTypes()
    {
        return NullTypes;
    }

    public static void RegisterCustomHandler(Type type, Func<object, IResult> handler)
    {
        CustomHandlers[type] = handler;
    }

    public static Func<object, IResult>? GetCustomHandler(Type type) =>
        CustomHandlers.TryGetValue(type, out var handler) ? handler : null;

    public void WithOk()
    {
        Ok = OkType.Ok;
    }

    public void WithCreated(string? uri)
    {
        Ok = OkType.Created;
        Uri = uri;
    }

    public void WithCreatedAtRoute(string? routeName, object? routeValues = null)
    {
        Ok = OkType.CreatedAtRoute;
        RouteName = routeName;
        RouteValues = routeValues;
    }

    public void WithAccepted(string? uri)
    {
        Ok = OkType.Accepted;
        Uri = uri;
    }

    public void WithAcceptedAtRoute(string? routeName, object? routeValues = null)
    {
        Ok = OkType.AcceptedAtRoute;
        RouteName = routeName;
        RouteValues = routeValues;
    }

    public void WithError(ErrorType errorType)
    {
        Error = errorType;
    }

    public void WithInvalid(InvalidType invalidType)
    {
        Invalid = invalidType;
    }

    public object Clone() =>
        new ToMinimalApiOptions
        {
            Ok = Ok,
            Error = Error,
            Invalid = Invalid
        };
}
namespace Peter.Result.MinimalApi;

public class ToMinimalApiOptions : ICloneable
{
    public OkType Ok { get; private set; }

    public ErrorType Error { get; private set; }

    public InvalidType Invalid { get; private set; }

    public string? Uri { get; private set; }

    public string? RouteName { get; private set; }

    public object? RouteValues { get; private set; }

    public ToMinimalApiConfiguration Configuration { get; private set; }

    private static readonly ToMinimalApiOptions DefaultOptions = new();

    private ToMinimalApiOptions()
    {
        Ok = OkType.Ok;
        Error = ErrorType.Problem;
        Invalid = InvalidType.Problem;
        Configuration = new ToMinimalApiConfiguration();
    }

    /// <summary>
    ///     Return default options for <see cref="ToMinimalApiOptions" />.
    /// </summary>
    /// <returns></returns>
    public static ToMinimalApiOptions GetDefaultOptions()
    {
        return DefaultOptions;
    }

    /// <summary>
    ///     Create a new instance of <see cref="ToMinimalApiOptions" /> to be used in any endpoint handler, using as initial
    ///     values the default options.
    /// </summary>
    /// <returns></returns>
    public static ToMinimalApiOptions Create()
    {
        return (ToMinimalApiOptions)DefaultOptions.Clone();
    }

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
            Invalid = Invalid,
            Configuration = (ToMinimalApiConfiguration)Configuration.Clone()
        };
}
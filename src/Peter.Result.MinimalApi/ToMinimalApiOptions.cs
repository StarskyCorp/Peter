namespace Peter.Result.MinimalApi;

public class ToMinimalApiOptions
{
    public bool UseProblemDetails { get; set; } = true;
    public Ok Ok { get; set; } = Ok.Ok;
    public string? RouteName { get; set; }
    public object? RouteValues { get; set; }

    public void CreatedAt(string routeName, object routeValues)
    {
        Ok = Ok.Created;
        RouteName = routeName;
        RouteValues = routeValues;
    }
}

public enum Ok
{
    Ok,
    Created
}


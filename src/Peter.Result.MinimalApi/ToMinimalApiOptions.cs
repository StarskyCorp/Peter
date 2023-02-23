namespace Peter.Result.MinimalApi;

public class ToMinimalApiOptions
{
    public string Route { get; private set; } = string.Empty;
    public object? RouteValues { get; private set; }
    public OkBehaviourType OkBehaviour { get; private set; } = OkBehaviourType.Ok;
    public NoContentBehaviourType NoContentBehaviour { get; private set; } = NoContentBehaviourType.NotFound;

    public bool UseProblemDetails { get; set; } = true;

    public void WithCreatedBehaviour(string route, object routeValues = null)
    {
        Route = route;
        RouteValues = routeValues;
        OkBehaviour = OkBehaviourType.Created;
    }

    public void WithCreatedAtBehaviour(string routeName, object routeValues = null)
    {
        Route = routeName;
        RouteValues = routeValues;
        OkBehaviour = OkBehaviourType.CreatedAt;
    }

    public void WithNoContentBehaviour() => NoContentBehaviour = NoContentBehaviourType.NoContent;
    public void WithNotFoundBehaviour() => NoContentBehaviour = NoContentBehaviourType.NotFound;
}
public enum OkBehaviourType
{
    Ok,
    Created,
    CreatedAt
}

public enum NoContentBehaviourType
{
    NoContent,
    NotFound
}
namespace Peter.Result.MinimalApi;

public class ToMinimalApiOptions
{
    public OkType Ok { get; private set; }
    public string Route { get; private set; }
    public object? RouteValues { get; private set; }
    public ErrorType Error { get; private set; }
    public NotFoundType NotFound { get; private set; }
    public InvalidType Invalid { get; private set; }

    public ToMinimalApiOptions()
    {
        Ok = OkType.Ok;
        Route = string.Empty;
        Error = ErrorType.Problem;
        NotFound = NotFoundType.NotFound;
        Invalid = InvalidType.ValidationProblem;
    }

    public void UseOk()
    {
        Ok = OkType.Ok;
    }

    public void UseCreated(string route, object? routeValues = null)
    {
        Ok = OkType.Created;
        Route = route;
        RouteValues = routeValues;
    }

    public void UseCreatedAtRoute(string routeName, object? routeValues = null)
    {
        Ok = OkType.CreatedAtRoute;
        Route = routeName;
        RouteValues = routeValues;
    }

    public void UseAccepted(string route)
    {
        Ok = OkType.Accepted;
        Route = route;
    }

    public void UseAcceptedAtRoute(string routeName, object? routeValues = null)
    {
        Ok = OkType.AcceptedAtRoute;
        Route = routeName;
        RouteValues = routeValues;
    }

    public void UseProblem()
    {
        Error = ErrorType.Problem;
    }

    public void UseInternalServerError()
    {
        Error = ErrorType.InternalServerError;
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

    public void UseBadRequest()
    {
        Invalid = InvalidType.BadRequest;
    }
}
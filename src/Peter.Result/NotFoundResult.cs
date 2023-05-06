namespace Peter.Result;

public class NotFoundResult<T> : Result<T>
{
    public NotFoundResult() : base(false, default)
    {
    }
}
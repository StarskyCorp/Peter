namespace Peter.Result;

public class NotFoundResult<T> : Result<T>
{
    public NotFoundResult(T? value = default) : base(false, value)
    {
    }
}
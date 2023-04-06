namespace Peter.Result;

public class NotFoundResult<T> : Result<T>
{
    protected NotFoundResult(T? value) : base(false, value)
    {
    }

    public static NotFoundResult<T> Create(T? value = default) =>
        new(value);
}
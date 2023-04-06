namespace Peter.Result;

public class NotFoundResult<T> : Result<T>
{
    private NotFoundResult(T? value) : base(false, value)
    {
    }

    public static NotFoundResult<T> Create(T? value = default) =>
        new(value);
}
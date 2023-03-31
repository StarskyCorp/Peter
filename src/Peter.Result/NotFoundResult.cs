namespace Peter.Result;

public sealed class NotFoundResult<T> : ResultBase<T>
{
    private NotFoundResult(T? value) : base(false, value)
    {
    }

    public static NotFoundResult<T> Create(T? value = default) =>
        new(value);
}
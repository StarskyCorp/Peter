namespace Peter.Result;

public sealed class NotExistsResult<T> : ResultBase<T>
{
    private NotExistsResult(T? value) : base(value, false)
    {
    }

    public static NotExistsResult<T> Create(T? value = default) =>
        new(value);
}
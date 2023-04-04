namespace Peter.Result.MinimalApi;

public sealed class NotExistResult<T> : Result<T>
{
    private NotExistResult(T? value) : base(false, value)
    {
    }

    public static NotExistResult<T> Create(T? value = default) =>
        new(value);
}
namespace Peter.Result;

public class NotExistsResult<T> : Result<T>
{
    protected NotExistsResult(T? value) : base(value, false)
    {
    }

    public static NotExistsResult<T> CreateNotExists(T? value = default) =>
        new(value);
}
namespace Peter.Result;

public class OkResult<T> : Result<T>
{
    private OkResult(T? value) : base(true, value)
    {
    }

    public static OkResult<T> Create(T? value = default)
    {
        return new OkResult<T>(value);
    }

    public static implicit operator OkResult<T>(T value) => Create(value);
}
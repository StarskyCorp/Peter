namespace Peter.Result;

public class OkResult<T> : Result<T>
{
    public OkResult(T? value = default) : base(true, value)
    {
    }

    public static implicit operator OkResult<T>(T value) => new(value);
}
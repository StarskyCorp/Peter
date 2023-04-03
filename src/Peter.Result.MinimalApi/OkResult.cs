namespace Peter.Result.MinimalApi;

public sealed class OkResult<T> : Result<T>
{
    private OkResult(T? value) : base(true, value)
    {
    }
    
    public static OkResult<T> Create(T? value = default) =>
        new(value);
    
    public static implicit operator bool(OkResult<T> result) => result.Success;

    public static implicit operator OkResult<T>(T value) => new(value);
}
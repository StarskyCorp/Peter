namespace Peter.Result;

public abstract class ResultBase<T>
{
    public bool Success { get; }
    public T? Value { get; }
    public IEnumerable<string>? Errors { get; }

    protected ResultBase(bool success, T? value)
    {
        Success = success;
        Value = value;
    }

    protected ResultBase(bool success, T? value, IEnumerable<string> errors)
    {
        Success = success;
        Value = value;
        Errors = errors;
    }

    public static implicit operator bool(ResultBase<T> result) => result.Success;
}
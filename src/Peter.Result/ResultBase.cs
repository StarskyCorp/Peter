namespace Peter.Result;

public abstract class ResultBase<T>
{
    public T? Value { get; }
    public bool Success { get; }

    public IEnumerable<string>? Errors { get; }

    protected ResultBase(T? value, bool success)
    {
        Value = value;
        Success = success;
    }

    protected ResultBase(T? value, bool success, IEnumerable<string> errors)
    {
        Value = value;
        Success = success;
        Errors = errors;
    }

    public static implicit operator bool(ResultBase<T> result) => result.Success;
}
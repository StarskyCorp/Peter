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

public class Result<T>: ResultBase<T>
{
    protected Result(T? value, bool success) : base(value, success)
    {
    }

    protected Result(T? value, bool success, IEnumerable<string> errors): base(value, success, errors)
    {
    }

    public static Result<T> CreateSuccess(T? value = default) => new(value, true);

    public static Result<T> CreateFailure(IEnumerable<string> errors, T? value = default) =>
        new(value, false, errors);
    
    public static implicit operator Result<T>(T value) => new(value, true);
}
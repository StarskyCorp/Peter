namespace Peter.Result;

public class Result<T>
{
    public T? Value { get; }
    public bool Success { get; }

    public IEnumerable<string>? Errors { get; }

    protected Result(T? value, bool success)
    {
        Value = value;
        Success = success;
    }

    protected Result(T? value, bool success, IEnumerable<string> errors)
    {
        Value = value;
        Success = success;
        Errors = errors;
    }

    public static Result<T> CreateSuccess(T? value = default) => new(value, true);

    public static Result<T> CreateFailure(IEnumerable<string> errors, T? value = default) =>
        new(value, false, errors);

    public static implicit operator bool(Result<T> result) => result.Success;

    public static implicit operator Result<T>(T value) => CreateSuccess(value);
}
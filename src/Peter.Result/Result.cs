namespace Peter.Result;

public class Result<T>
{
    public bool Success { get; }
    public T? Value { get; }
    public IEnumerable<Error>? Errors { get; }

    protected Result(bool success, T? value)
    {
        Success = success;
        Value = value;
    }

    protected Result(bool success, T? value, IEnumerable<Error>? errors)
    {
        Success = success;
        Value = value;
        Errors = errors;
    }


    public static Result<T> CreateSuccess(T? value = default) => new(true, value);

    public static Result<T> CreateFailure(T? value, IEnumerable<Error> errors) =>
        new(false, value, errors);

    public static Result<T> CreateFailure(IEnumerable<Error> errors) =>
        CreateFailure(default, errors);

    public static implicit operator bool(Result<T> result) => result.Success;
    
    public static implicit operator Result<T>(T value) => new(true, value);
}
namespace Peter.Result;

public class Result<T>
{
    public bool Ok { get; }
    public T? Value { get; }
    public IEnumerable<Error>? Errors { get; }

    protected Result(bool ok)
    {
        Ok = ok;
    }
    
    protected Result(bool ok, T? value): this(ok)
    {
        Value = value;
    }

    protected Result(bool ok, T? value, IEnumerable<Error>? errors): this(ok, value)
    {
        Errors = errors;
    }

    public static Result<T> CreateOk(T? value = default) => new(true, value);

    public static Result<T> CreateError(T? value = default) =>
        CreateError(value, Enumerable.Empty<Error>());

    public static Result<T> CreateError(IEnumerable<Error> errors) =>
        CreateError(default, errors);

    public static Result<T> CreateError(T? value, IEnumerable<Error> errors) =>
        new(false, value, errors);

    public static implicit operator bool(Result<T> result) => result.Ok;
    
    public static implicit operator Result<T>(T value) => new(true, value);
}
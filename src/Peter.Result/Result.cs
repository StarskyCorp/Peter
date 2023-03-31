namespace Peter.Result;

public sealed class Result<T>: ResultBase<T>
{
    private Result(bool success, T? value) : base(success, value)
    {
    }

    private Result(bool success, T? value, IEnumerable<string> errors): base(success, value, errors)
    {
    }

    public static Result<T> CreateSuccess(T? value = default) => new(true, value);

    public static Result<T> CreateFailure(IEnumerable<string> errors, T? value = default) =>
        new(false, value, errors);
    
    public static implicit operator Result<T>(T value) => new(true, value);
}
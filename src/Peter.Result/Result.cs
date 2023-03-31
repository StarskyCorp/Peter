namespace Peter.Result;

public sealed class Result<T>: ResultBase<T>
{
    private Result(T? value, bool success) : base(value, success)
    {
    }

    private Result(T? value, bool success, IEnumerable<string> errors): base(value, success, errors)
    {
    }

    public static Result<T> CreateSuccess(T? value = default) => new(value, true);

    public static Result<T> CreateFailure(IEnumerable<string> errors, T? value = default) =>
        new(value, false, errors);
    
    public static implicit operator Result<T>(T value) => new(value, true);
}
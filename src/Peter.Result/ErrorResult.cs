namespace Peter.Result;

public class ErrorResult<T> : Result<T>
{
    public IEnumerable<Error> Errors { get; }

    private ErrorResult(T? value, IEnumerable<Error>? errors) : base(false, value)
    {
        Errors = errors ?? Enumerable.Empty<Error>();
    }

    public static ErrorResult<T> Create(IEnumerable<Error>? errors = default, T? value = default)
    {
        return new ErrorResult<T>(value, errors);
    }

    public static ErrorResult<T> Create(string message, T? value = default)
    {
        return Create(new[] { new Error(message) }, value);
    }
}
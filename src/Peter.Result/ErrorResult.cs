namespace Peter.Result;

public class ErrorResult<T> : Result<T>
{
    public IEnumerable<Error> Errors { get; }

    public ErrorResult(T? value, IEnumerable<Error>? errors) : base(false, value)
    {
        Errors = errors ?? Enumerable.Empty<Error>();
    }

    public ErrorResult(IEnumerable<Error>? errors = default, T? value = default) : this(value, errors)
    {
    }

    public ErrorResult(string message, T? value = default) : this(value, new[] { new Error(message) })
    {
    }
}
namespace Peter.Result;

public class ErrorResult<T> : Result<T>
{
    public string? Error { get; }

    public ErrorResult(string? error) : base(false, default)
    {
        Error = error;
    }

    public ErrorResult() : this(default)
    {
    }
}
namespace Peter.Result;

public class Result<T>
{
    public bool Ok { get; }
    public T? Value { get; }

    public Result(bool ok)
    {
        Ok = ok;
    }

    public Result(bool ok, T? value) : this(ok)
    {
        Value = value;
    }

    public static implicit operator bool(Result<T> result) => result.Ok;
}
namespace Peter.Result;

public abstract class Result<T>
{
    public bool Ok { get; }
    public T? Value { get; }

    protected Result(bool ok)
    {
        Ok = ok;
    }

    protected Result(bool ok, T? value) : this(ok)
    {
        Value = value;
    }

    public static implicit operator bool(Result<T> result) => result.Ok;
}
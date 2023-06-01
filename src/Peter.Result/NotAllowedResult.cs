namespace Peter.Result;

public class NotAllowedResult<T> : Result<T>
{
    public NotAllowedResult() : base(false, default)
    {
    }
}
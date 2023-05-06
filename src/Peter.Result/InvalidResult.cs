namespace Peter.Result;

public class InvalidResult<T> : Result<T>
{
    public InvalidResult() : base(false)
    {
    }
}
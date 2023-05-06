namespace Peter.Result;

public class SimpleInvalidResult<T> : InvalidResult<T>
{
    public string Message { get; }


    public SimpleInvalidResult(string message)
    {
        Message = message;
    }
}
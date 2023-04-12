namespace Peter.Result;

public class Error
{
    public string Message { get; }

    public Error(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        Message = message;
    }
}
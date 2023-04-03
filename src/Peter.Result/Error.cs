namespace Peter.Result;

public class Error
{
    public Error(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public string Message { get; }
}
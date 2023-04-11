namespace Peter.Result;

public class Error
{
    public string Message { get; }

    public Error(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }
}
namespace Peter.Result;

public class ValidationError
{
    public ValidationError(string field, string message)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public string Field { get; }
    public string Message { get; }
}
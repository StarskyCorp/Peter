namespace Peter.Result;

public class ValidationError : Error
{
    public ValidationError(string field, string message) : base(message)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public string Field { get; }
}
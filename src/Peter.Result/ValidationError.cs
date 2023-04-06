namespace Peter.Result;

public class ValidationError : Error
{
    public string Identifier { get; }

    public ValidationError(string identifier, string message) : base(message)
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
    }
}
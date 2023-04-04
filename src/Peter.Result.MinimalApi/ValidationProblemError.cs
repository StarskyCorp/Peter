namespace Peter.Result.MinimalApi;

public class ValidationProblemError : Error
{
    public string Identifier { get; }

    public ValidationProblemError(string identifier, string message) : base(message)
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
    }
}
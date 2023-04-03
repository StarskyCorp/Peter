namespace Peter.Result.MinimalApi;

public class ValidationProblemError : Error
{
    public ValidationProblemError(string field, string message) : base(message)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public string Field { get; }
}
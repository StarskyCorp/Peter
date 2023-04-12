namespace Peter.Result;

public class InvalidResult<T> : Result<T>
{
    public IEnumerable<ValidationError> ValidationErrors { get; }

    public InvalidResult(T? value, IEnumerable<ValidationError>? validationErrors) : base(false, value)
    {
        ValidationErrors = validationErrors ?? Enumerable.Empty<ValidationError>();
    }

    public InvalidResult(IEnumerable<ValidationError>? validationErrors = default, T? value = default) : this(value,
        validationErrors)
    {
    }

    public InvalidResult(string identifier, string message, T? value = default) : this(value,
        new ValidationError[] { new(identifier, message) })
    {
    }
}
namespace Peter.Result;

public sealed class InvalidResult<T> : ResultBase<T>
{
    public IEnumerable<ValidationError>? ValidationErrors { get; }

    private InvalidResult(T? value, IEnumerable<ValidationError> validationErrors) : base(value, false) =>
        ValidationErrors = validationErrors;

    public static InvalidResult<T> Create(IEnumerable<ValidationError> validationErrors, T? value = default) =>
        new(value, validationErrors);
}

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
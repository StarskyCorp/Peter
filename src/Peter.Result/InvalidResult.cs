namespace Peter.Result;

public class InvalidResult<T> : Result<T>
{
    public IEnumerable<ValidationError>? ValidationErrors { get; }

    protected InvalidResult(T? value, IEnumerable<ValidationError> validationErrors) : base(value, false) =>
        ValidationErrors = validationErrors;

    public static InvalidResult<T> CreateInvalid(IEnumerable<ValidationError> validationErrors, T? value = default) =>
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
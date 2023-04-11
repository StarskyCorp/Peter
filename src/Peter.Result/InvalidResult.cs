namespace Peter.Result;

public class InvalidResult<T> : Result<T>
{
    public IEnumerable<ValidationError> ValidationErrors { get; }

    protected InvalidResult(T? value, IEnumerable<ValidationError>? validationErrors) :
        base(false, value)
    {
        ValidationErrors = validationErrors ?? Enumerable.Empty<ValidationError>();
    }

    public static InvalidResult<T> Create(IEnumerable<ValidationError>? validationErrors = default,
        T? value = default) =>
        new(value, validationErrors);

    public static InvalidResult<T> Create(string identifier, string message,
        T? value = default) =>
        new(value, new ValidationError[] { new(identifier, message) });
}
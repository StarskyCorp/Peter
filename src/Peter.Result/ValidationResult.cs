namespace Peter.Result;

public sealed class ValidationResult<T> : ResultBase<T>
{
    public IEnumerable<ValidationError>? ValidationErrors { get; }

    private ValidationResult(T? value, IEnumerable<ValidationError> validationErrors, bool success) :
        base(value, success) =>
        ValidationErrors = validationErrors;

    public static ValidationResult<T> Create(IEnumerable<ValidationError> validationErrors, T? value = default,
        bool success = false) =>
        new(value, validationErrors, success);
}
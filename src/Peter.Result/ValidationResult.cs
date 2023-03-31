namespace Peter.Result;

public sealed class ValidationResult<T> : ResultBase<T>
{
    public IEnumerable<ValidationError>? ValidationErrors { get; }

    private ValidationResult(bool success, T? value, IEnumerable<ValidationError>? validationErrors) :
        base(success, value) =>
        ValidationErrors = validationErrors;

    public static ValidationResult<T> Create(bool success, T? value = default,
        IEnumerable<ValidationError>? validationErrors = null)
    {
        if (!success)
        {
            ArgumentNullException.ThrowIfNull(validationErrors, nameof(validationErrors));
        }

        return new ValidationResult<T>(success, value, validationErrors);
    }
}
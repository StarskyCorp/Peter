namespace Peter.Result;

public sealed class InvalidResult<T> : ResultBase<T>
{
    public IEnumerable<ValidationError> ValidationErrors { get; }

    private InvalidResult(T? value, IEnumerable<ValidationError> validationErrors) :
        base(false, value) =>
        ValidationErrors = validationErrors;

    public static InvalidResult<T> Create(IEnumerable<ValidationError> validationErrors, T? value = default) =>
        new(value, validationErrors);
}
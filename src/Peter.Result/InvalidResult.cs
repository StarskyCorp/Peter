namespace Peter.Result;

public sealed class InvalidResult<T> : ResultBase<T>
{
    private InvalidResult(T? value, IEnumerable<ValidationError> errors) :
        base(false, value, errors)
    {
    }

    public static InvalidResult<T> Create(IEnumerable<ValidationError> errors, T? value = default) =>
        new(value, errors);
}
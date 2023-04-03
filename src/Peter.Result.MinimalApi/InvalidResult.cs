namespace Peter.Result.MinimalApi;

public sealed class InvalidResult<T> : Result<T>
{
    private InvalidResult(T? value, IEnumerable<ValidationProblemError> errors) :
        base(false, value, errors)
    {
    }

    public static InvalidResult<T> Create(IEnumerable<ValidationProblemError> errors, T? value = default) =>
        new(value, errors);
}
namespace Peter.Result;

public class Result<T>
{
    public T? Value { get; }
    public ResultStatus Status { private init; get; }
    public bool Success => Status == ResultStatus.Success;
    public IEnumerable<string>? Errors { get; private init; }
    public IEnumerable<ValidationError>? ValidationErrors { get; private init; }

    private Result(T? value)
    {
        Value = value;
        Status = ResultStatus.Success;
    }

    public static Result<T> CreateSuccess(T? value = default) => new(value);

    public static Result<T> CreateFailure(IEnumerable<string> errors, T? value = default) =>
        new(value) { Status = ResultStatus.Failure, Errors = errors };

    public static Result<T> CreateNotExists(T? value = default) =>
        new(value) { Status = ResultStatus.NotExists };

    public static Result<T> CreateInvalid(IEnumerable<ValidationError> validationErrors, T? value = default) =>
        new(value) { Status = ResultStatus.Invalid, ValidationErrors = validationErrors };

    public static implicit operator bool(Result<T> result) => result.Success;

    public static implicit operator Result<T>(T value) => CreateSuccess(value);
}
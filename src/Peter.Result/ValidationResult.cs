namespace Peter.Result;

public sealed class ValidationResult
{
    public bool IsValid { get; }

    public IEnumerable<Error>? Errors { get; }

    private ValidationResult(bool isValid, IEnumerable<Error>? errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    public static ValidationResult Create(bool isValid, IEnumerable<Error>? errors = null)
    {
        if (!isValid)
        {
            ArgumentNullException.ThrowIfNull(errors, nameof(errors));
        }

        return new ValidationResult(isValid, errors);
    }

    public InvalidResult<object> ToInvalidResult()
    {
        return ToInvalidResult<object>();
    }
    
    public InvalidResult<T> ToInvalidResult<T>(T? value = default)
    {
        var errors = new List<ValidationError>();
        foreach (var error in Errors!)
        {
            switch (error)
            {
                case ValidationError validationError:
                    errors.Add(validationError);
                    break;
                default:
                    errors.Add(new ValidationError(string.Empty, error.Message));
                    break;
            }
        }

        return InvalidResult<T>.Create(errors, value);
    }
}
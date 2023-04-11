using Peter.Result;

namespace Api;

/// <summary>
///     A specialized version of <see cref="OkResult" />
/// </summary>
public class VeryOkResult<T> : OkResult<T>
{
    protected VeryOkResult(T? value) : base(value)
    {
    }

    public new static VeryOkResult<T> Create(T? value = default)
    {
        return new VeryOkResult<T>(value);
    }

    public static implicit operator VeryOkResult<T>(T value) => new(value);
}
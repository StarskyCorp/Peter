using Peter.Result;

namespace Api;

/// <summary>
///     A specialized version of <see cref="OkResult" />
/// </summary>
public class VeryOkResult<T> : OkResult<T>
{
    public VeryOkResult(T? value = default) : base(value)
    {
    }

    public static implicit operator VeryOkResult<T>(T value) => new(value);
}
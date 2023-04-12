using Peter.Result;

namespace Api;

public class TeapotResult<T> : Result<T>
{
    public TeapotResult(bool ok, T? value = default) : base(ok, value)
    {
    }
}
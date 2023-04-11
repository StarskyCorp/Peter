using Peter.Result;

namespace Api;

public class TeapotResult<T> : Result<T>
{
    protected TeapotResult(bool ok, T? value) : base(ok, value)
    {
    }

    public static TeapotResult<T> Create(bool ok, T? value = default)
    {
        return new TeapotResult<T>(ok, value);
    }
}
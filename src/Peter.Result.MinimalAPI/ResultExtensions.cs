using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Peter.Result;

public static class ResultExtensions
{
    public static IResult ToMinimalApi<T>(this Result<T> result, Action<ToMinimalApiOptions> configure)
    {
        //TODO: Options 
        var options = new ToMinimalApiOptions();
        configure(options);

        return result.Status switch
        {
            ResultStatus.Success => Results.Ok(result.Value),
            ResultStatus.Failure => Results.Problem(), // TODO: check this case 
            ResultStatus.NotExists => Results.NotFound(result.Value),
            ResultStatus.Invalid => Results.ValidationProblem(result.ToProblemDetails()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static IResult ToMinimalApi<T>(this Result<T> result)
    {
        return result.ToMinimalApi(_ => { });
    }

    public static IDictionary<string, string[]> ToProblemDetails<T>(this Result<T> result)
    {
        var details = result.ValidationErrors?
            .GroupBy(x => x.Field)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Message).ToArray());

        return details ?? new Dictionary<string, string[]>();
    }
}

public class ToMinimalApiOptions
{
}
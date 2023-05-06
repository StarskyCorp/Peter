using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Peter.Result.MinimalApi;

public class InternalServerErrorResult : IResult
{
    private readonly object? _content;

    public InternalServerErrorResult(object? content)
    {
        _content = content;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        if (_content is not null)
        {
            await httpContext.Response.WriteAsJsonAsync(_content);
        }
    }
}
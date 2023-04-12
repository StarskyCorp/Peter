using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Peter.Result.MinimalApi;

class InternalServerErrorResult : IResult
{
    private readonly string _content;

    public InternalServerErrorResult(string content)
    {
        _content = content;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_content);
        return httpContext.Response.WriteAsync(_content);
    }
}
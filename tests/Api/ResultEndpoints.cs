using Microsoft.AspNetCore.Mvc;
using Peter.Result;

namespace Api;

public static class ResultEndpoints
{
    public static IEndpointRouteBuilder AddResultEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/ok", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi();
        });

        app.MapPost("/created", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseCreated("/any_url"));
        });

        app.MapPost("/created_at_route", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseCreatedAtRoute("GetFoo", new { id = 1 }));
        });

        app.MapPost("/accepted", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseAccepted("/any_url"));
        });

        app.MapPost("/accepted_at_route", ([FromBody] string payload) =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.UseAcceptedAtRoute("GetFoo", new { id = 2 }));
        });

        app.MapGet("/foo/{id:int}", (int id) => { }).WithName("GetFoo");

        app.MapGet("/very_ok", () =>
        {
            var result = new VeryOkResult<string>("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/problem", () =>
        {
            var result = new ErrorResult<object>(new[] { new Error("A failure") });
            return result.ToMinimalApi();
        });

        app.MapGet("/internal_server_error", () =>
        {
            var result = new ErrorResult<object>("A failure");
            return result.ToMinimalApi(options => { options.UseInternalServerError(); });
        });

        app.MapGet("/not_found", () =>
        {
            var result = new NotFoundResult<object>("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/no_content", () =>
        {
            var result = new NotFoundResult<object>();
            return result.ToMinimalApi(options => options.UseNoContent());
        });

        app.MapGet("/validation_problem", () =>
        {
            var result = new InvalidResult<object>("peter", "message");
            return result.ToMinimalApi();
        });

        app.MapGet("/bad_request", (bool simple) =>
        {
            var result = new InvalidResult<object>("peter", "message");
            if (!simple)
            {
                return result.ToMinimalApi(options => options.UseBadRequest());
            }

            return result.ToMinimalApi(options => options.UseBadRequest(simple: true));
        });

        app.MapGet("/open_teapot", (bool ok) => new TeapotResult<string>(ok, "Peter").ToMinimalApi());

        app.MapGet("/closed_teapot", (int age) =>
        {
            var result = new TeapotResult<int>(true, age);
            return result.ToMinimalApi();
        });

        app.MapGet("/using_result_type_base", (bool ok, bool toString) =>
        {
            var result = new Result<object>(ok, "Peter");
            return result.ToMinimalApi(options => { options.UseInternalServerError(toString); });
        });

        return app;
    }
}
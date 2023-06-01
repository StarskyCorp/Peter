using Peter.Result;
using Peter.Result.MinimalApi;

namespace Api;

public static class ResultEndpoints
{
    public static IEndpointRouteBuilder AddResultEndpoints(this IEndpointRouteBuilder app)
    {
        #region Ok

        app.MapGet("/ok", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi();
        });

        app.MapPost("/created", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithCreated("/peter"));
        });

        app.MapPost("/created_at_route", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithCreatedAtRoute("GetPeter", new { id = 1 }));
        });

        app.MapGet("/peter/{id:int}", (int id) => { }).WithName("GetPeter");

        app.MapPost("/accepted", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithAccepted("/peter"));
        });

        app.MapPost("/accepted_at_route", () =>
        {
            OkResult<string> result = "Peter";
            return result.ToMinimalApi(options => options.WithAcceptedAtRoute("GetPeter", new { id = 1 }));
        });

        app.MapGet("/ok_using_very_ok_type", () =>
        {
            var result = new VeryOkResult<string>("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/ok_using_result_type", () =>
        {
            var result = new Result<object>(true, "Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/ok_with_no_value", () =>
        {
            var result = new OkResult<string>();
            return result.ToMinimalApi();
        });

        #endregion

        #region Error

        app.MapGet("/error", () =>
        {
            var result = new ErrorResult<object>("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/error_without_detail", () =>
        {
            var result = new ErrorResult<object>();
            return result.ToMinimalApi();
        });

        app.MapGet("/internal_server_error", () =>
        {
            var result = new ErrorResult<object>("Peter");
            return result.ToMinimalApi(options => { options.WithError(ErrorType.InternalServerError); });
        });

        app.MapGet("/internal_server_error_without_body", () =>
        {
            var result = new ErrorResult<object>();
            return result.ToMinimalApi(options => { options.WithError(ErrorType.InternalServerError); });
        });

        app.MapGet("/error_using_result_type_without_body", () =>
        {
            var result = new Result<object>(false);
            return result.ToMinimalApi();
        });

        #endregion

        #region NotFound

        app.MapGet("/not_found", () =>
        {
            var result = new NotFoundResult<object>();
            return result.ToMinimalApi();
        });

        #endregion

        #region Invalid

        app.MapGet("/bad_request_using_problem_details_with_simple_invalid_result_type", () =>
        {
            var result = new SimpleInvalidResult<object>("Peter");
            return result.ToMinimalApi();
        });

        app.MapGet("/bad_request_using_validation_problem_details_with_detailed_invalid_result_type", () =>
        {
            var result = new DetailedInvalidResult<object>("Peter", "Starsky");
            return result.ToMinimalApi();
        });

        app.MapGet("/bad_request_with_simple_invalid_result_type", () =>
        {
            var result = new SimpleInvalidResult<object>("Peter");
            return result.ToMinimalApi(options => options.WithInvalid(InvalidType.BadRequest));
        });

        app.MapGet("/bad_request_with_detailed_invalid_result_type", () =>
        {
            var result = new DetailedInvalidResult<object>("Peter", "Starsky");
            return result.ToMinimalApi(options => options.WithInvalid(InvalidType.BadRequest));
        });

        #endregion
        
        #region NotAllowed

        app.MapGet("/not_allowed", () =>
        {
            var result = new NotAllowedResult<object>();
            return result.ToMinimalApi();
        });

        #endregion        

        #region Teapot

        app.MapGet("/teapot", (bool ok, string? owner) => new TeapotResult<string>(ok, owner).ToMinimalApi());

        app.MapGet("/aged_teapot", (int age) =>
        {
            var result = new TeapotResult<int>(true, age);
            return result.ToMinimalApi();
        });

        #endregion

        return app;
    }
}
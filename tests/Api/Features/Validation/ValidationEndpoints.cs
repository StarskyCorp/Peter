using Peter.MinimalApi.Validation;

namespace Api.Features.Validation;

public static class ValidationEndpoints
{
    public static IEndpointRouteBuilder AddValidationEndpoints(this IEndpointRouteBuilder app)
    {
        #region Validated<T>

        app.MapPost("/validate_using_validated_generic_type",
            (Validated<Product> product) =>
                !product.IsValid ? Results.ValidationProblem(product.Errors) : Results.Ok(product.Value));

        app.MapPost("/fail_validation_using_validated_generic_type_when_there_is_not_a_custom_validator_registered",
            (Validated<ProductWithoutCustomValidator> product) => !product.IsValid
                ? Results.ValidationProblem(product.Errors)
                : Results.Ok(product.Value));

        #endregion

        #region ValidationFilter.ValidationEndpointFilterFactory + [Validate]

        app.MapPost("/validate_using_validate_attribute", ([Validate] Product product) => product);

        // This endpoint shows that it's not required Validate attribute if you don't want
        app.MapPost("/validate_omitting_validate_attribute", (Product product) => product);

        app.MapPost("/fail_validation_using_validate_attribute_when_there_is_not_a_custom_validator_registered",
            (ProductWithoutCustomValidator product) => product);

        #endregion

        return app;
    }
}
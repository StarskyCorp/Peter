using Peter.MinimalApi.Validation;

namespace Api.Features.Validation;

public static class ValidationEndpoints
{
    public static IEndpointRouteBuilder AddValidationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/validate_using_validated_generic_type",
            (Validated<Product> product) =>
                !product.IsValid ? Results.ValidationProblem(product.Errors) : Results.Ok(product.Value));

        app.MapPost("/fail_validation_using_validated_generic_type_when_there_is_not_a_custom_validator_registered",
            (Validated<ProductWithoutCustomValidator> product) => !product.IsValid
                ? Results.ValidationProblem(product.Errors)
                : Results.Ok(product.Value));

        // app.MapPost("/validate_using_validate_attribute", ([Validate] Product product) => Results.Ok(product))
        //     .AddEndpointFilterFactory(ValidationFilter.ValidationEndpointFilterFactory);
        //
        // app.MapPost("/fail_validation_using_validate_attribute_when_there_is_not_a_custom_validator_registered",
        //         ([Validate] ProductWithoutCustomValidator product) => Results.Ok(product))
        //     .AddEndpointFilterFactory(ValidationFilter.ValidationEndpointFilterFactory);
        
        app.MapPost("/validate_using_validate_attribute", (Product product) => Results.Ok(product));

        app.MapPost("/fail_validation_using_validate_attribute_when_there_is_not_a_custom_validator_registered",
                (ProductWithoutCustomValidator product) => Results.Ok(product));        

        return app;
    }
}
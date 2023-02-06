using Peter.MinimalApi.Validation;

namespace GreetingsApi.Features.Validation;

public static class ValidationEndpoints
{
    public static IEndpointRouteBuilder AddValidationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/validate_using_validated", (Validated<Product> product) =>
        {
            if (!product.IsValid)
            {
                return Results.ValidationProblem(product.Errors);
            }

            return Results.Ok(product.Value);
        });

        app.MapPost("/fail_validation_using_validated_when_there_is_not_a_custom_validator_registered",
            (Validated<ProductWithoutCustomValidator> product) =>
            {
                if (!product.IsValid)
                {
                    return Results.ValidationProblem(product.Errors);
                }

                return Results.Ok(product.Value);
            });

        app.MapPost("/validate_using_attribute", ([Validate] Product product) => Results.Ok(product))
            .AddEndpointFilterFactory(ValidationFilter.ValidationEndpointFilterFactory);

        app.MapPost("/fail_validation_using_attribute_when_there_is_not_a_custom_validator_registered",
                ([Validate] ProductWithoutCustomValidator product) => Results.Ok(product))
            .AddEndpointFilterFactory(ValidationFilter.ValidationEndpointFilterFactory);

        return app;
    }
}
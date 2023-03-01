using FluentValidation;

namespace Api.Features.Validation;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(customer => customer.Id).GreaterThan(0);
        RuleFor(customer => customer.Name).NotEmpty();
    }
}
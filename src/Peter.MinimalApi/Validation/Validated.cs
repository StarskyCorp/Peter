using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Peter.MinimalApi.Validation;

/// <summary>
/// Complex type for automatically validating a binding parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>https://khalidabuhakmeh.com/minimal-api-validation-with-fluentvalidation</remarks>
public class Validated<T>
{
    private ValidationResult ValidationResult { get; }

    private Validated(T value, ValidationResult validationResult)
    {
        Value = value;
        ValidationResult = validationResult;
    }

    public T Value { get; }
    public bool IsValid => ValidationResult.IsValid;

    public IDictionary<string, string[]> Errors => ValidationResult.ToDictionary();

    public void Deconstruct(out bool isValid, out T value)
    {
        isValid = IsValid;
        value = Value;
    }

    public static async ValueTask<Validated<T>> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var value = await context.Request.ReadFromJsonAsync<T>();
        if (value is null)
        {
            throw new ArgumentException($"{parameter.Name} cannot be null.");
        }
        IValidator<T>? validator = context.RequestServices.GetRequiredService<IValidator<T>>();
        var validationResult = await validator.ValidateAsync(value);
        return new Validated<T>(value, validationResult);
    }
}
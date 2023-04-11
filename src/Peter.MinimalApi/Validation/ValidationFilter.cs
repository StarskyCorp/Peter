using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Peter.MinimalApi.Validation;

/// <summary>
///     Automatic validation using an endpoint filter factory.
/// </summary>
/// <remarks>Inspired by https://benfoster.io/blog/minimal-api-validation-endpoint-filters/</remarks>
public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationEndpointFilterFactory(EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {
        var validationDescriptors =
            GetValidationDescriptors(context);

        return validationDescriptors.Any()
            ? invocationContext => Validate(validationDescriptors, invocationContext, next)
            : next;
    }

    private static IEnumerable<ValidationDescriptor> GetValidationDescriptors(EndpointFilterFactoryContext context)
    {
        var parameters = context.MethodInfo.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (!HasToValidate(parameter))
            {
                continue;
            }

            yield return new ValidationDescriptor
            {
                Name = parameter.Name!, Index = i, Type = parameter.ParameterType,
                ValidatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType)
            };
        }
    }

    private static bool HasToValidate(ParameterInfo parameter)
    {
        var hasToValidate = parameter.GetCustomAttribute<ValidateAttribute>() is not null;
        if (!hasToValidate)
        {
            var genericType = typeof(AbstractValidator<>).MakeGenericType(parameter.ParameterType);
            hasToValidate = parameter.ParameterType.GetNestedTypes().Any(t => t.IsSubclassOf(genericType));
            if (!hasToValidate)
            {
                try
                {
                    hasToValidate = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes(), (assembly, type) => type)
                        .Any(type => type.IsSubclassOf(genericType));
                }
                catch
                {
                    //Temporal fix: previous sentence fails in GitHub pipeline but not in Ubuntu 22.04 on WSL 2
                    //TODO: investigate
                }
            }
        }

        return hasToValidate;
    }

    private static async ValueTask<object?> Validate(IEnumerable<ValidationDescriptor> validationDescriptors,
        EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.Index]!;
            var validator =
                (IValidator)invocationContext.HttpContext.RequestServices
                    .GetRequiredService(descriptor.ValidatorType);
            var validationResult = await validator.ValidateAsync(
                new ValidationContext<object>(argument)
            );
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
        }

        return await next.Invoke(invocationContext);
    }

    private class ValidationDescriptor
    {
        public required string Name { get; init; }
        public required int Index { get; init; }
        public required Type Type { get; init; }
        public required Type ValidatorType { get; init; }
    }
}
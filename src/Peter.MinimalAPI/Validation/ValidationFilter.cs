﻿using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Peter.MinimalApi.Validation;

/// <summary>
/// Validation using an attribute and an endpoint filter factory.
/// </summary>
/// <remarks>https://benfoster.io/blog/minimal-api-validation-endpoint-filters/</remarks>
public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationEndpointFilterFactory(EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {
        var validationDescriptors =
            GetValidationDescriptors(context.MethodInfo);

        return validationDescriptors.Any()
            ? invocationContext => Validate(validationDescriptors, invocationContext, next)
            : next;
    }

    private static IEnumerable<ValidationDescriptor> GetValidationDescriptors(MethodBase methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            if (parameter.GetCustomAttribute<ValidateAttribute>() is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
            yield return new ValidationDescriptor
            { Name = parameter.Name!, Index = i, Type = parameter.ParameterType, ValidatorType = validatorType };
        }
    }

    private static async ValueTask<object?> Validate(IEnumerable<ValidationDescriptor> validationDescriptors,
        EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.Index];
            var validator =
                invocationContext.HttpContext.RequestServices
                    .GetRequiredService(descriptor.ValidatorType) as IValidator;
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
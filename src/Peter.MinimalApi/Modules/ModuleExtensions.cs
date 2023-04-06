using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Peter.MinimalApi.Modules;

/// <summary>
///     Carter inspired solution for minimal api modules organization.
/// </summary>
/// <remarks>https://timdeschryver.dev/blog/maybe-its-time-to-rethink-our-project-structure-with-dot-net-6</remarks>
public static class ModuleExtensions
{
    public static IEndpointRouteBuilder MapModules<T>(this IEndpointRouteBuilder app) where T : class
    {
        foreach (var module in GetModules<T>())
        {
            module.MapEndpoints(app);
        }

        return app;
    }

    private static IEnumerable<IModule> GetModules<T>() where T : class
    {
        var assembly = Assembly.GetAssembly(typeof(T));
        if (assembly is null)
        {
            return Enumerable.Empty<IModule>();
        }

        return assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}
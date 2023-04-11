using Peter.Result.MinimalApi;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ConfigureToMinimalApi(
        this IApplicationBuilder app,
        Action<ToMinimalApiOptions> configure)
    {
        configure(ToMinimalApiOptions.GetDefaultOptions());
        return app;
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Blazor.Utils.InteropEventListener.Registrars;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Drawflow.Registrars;

/// <summary>
/// A Blazor interop library for drawflow.js
/// </summary>
public static class DrawflowInteropRegistrar
{
    /// <summary>
    /// Adds <see cref="IDrawflowInterop"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddDrawflowInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped()
                .AddInteropEventListenerAsScoped()
                .TryAddScoped<IDrawflowInterop, DrawflowInterop>();

        return services;
    }
}

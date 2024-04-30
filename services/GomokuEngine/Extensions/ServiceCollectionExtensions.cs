using Microsoft.Extensions.DependencyInjection;

namespace GameHub.GomokuEngine.Extensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddGomokuGameEngine(this IServiceCollection services)
    {
        services.AddSingleton<GomokuManager>();
        return services;
    }
}
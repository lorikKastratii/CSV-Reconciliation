using CSVReconciliation.Core.Models;
using CSVReconciliation.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSVReconciliation.Console;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReconciliationServices(this IServiceCollection services, ReconciliationSettings settings)
    {
        services.AddSingleton(settings);
        services.AddSingleton<ConfigLoader>();
        services.AddSingleton<FilePairFinder>();
        services.AddSingleton(provider =>
        {
            var configLoader = provider.GetRequiredService<ConfigLoader>();
            return configLoader.Load(settings.ConfigPath);
        });
        services.AddTransient<AppLogger>();
        services.AddTransient<ResultWriter>();
        services.AddTransient<ReconciliationRunner>();

        return services;
    }
}

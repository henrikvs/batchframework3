using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Batch.DataAccess;

/// <summary>
/// Provides dependency injection registration helpers for the data access layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the batch framework data access services.
    /// </summary>
    public static IServiceCollection AddBatchDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BatchDatabase");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'BatchDatabase' is required.");
        }

        services.AddDbContext<BatchDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IAdministrationDA, AdministrationDA>();
        services.AddScoped<IOrchestrationDA, OrchestrationDA>();
        return services;
    }
}

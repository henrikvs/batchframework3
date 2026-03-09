using Batch.Services.Scheduling;
using Microsoft.Extensions.DependencyInjection;

namespace Batch.Services;

/// <summary>
/// Provides dependency injection registration helpers for application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the batch framework application services.
    /// </summary>
    public static IServiceCollection AddBatchServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ScheduleCalculator>();
        services.AddScoped<IAdministrationService, AdministrationService>();
        services.AddScoped<IRunRequestService, RunRequestService>();
        services.AddScoped<IJobInstanceService, JobInstanceService>();
        services.AddScoped<ISchedulerService, SchedulerService>();
        services.AddScoped<IRequestFiringService, RequestFiringService>();
        return services;
    }
}

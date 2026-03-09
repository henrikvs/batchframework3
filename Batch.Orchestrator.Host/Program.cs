using Batch.DataAccess;
using Batch.Orchestrator.Host;
using Batch.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddBatchDataAccess(builder.Configuration);
builder.Services.AddBatchServices();
builder.Services.AddHostedService<SchedulerBackgroundService>();
builder.Services.AddHostedService<RequestFiringBackgroundService>();

var host = builder.Build();
host.Run();

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Batch.DataAccess;
using Batch.Services;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddBatchDataAccess(builder.Configuration);
builder.Services.AddBatchServices();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(
            new
            {
                title = exception is ValidationException ? "Validation error" : "Request failed",
                status = statusCode,
                detail = exception?.Message,
            });
    });
});

app.MapOpenApi();
app.MapControllers();

app.Run();

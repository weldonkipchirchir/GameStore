using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Dtos.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandling(this IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("GlobalExceptionHandler");
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            logger.LogError(exception, "Could not process a request on machine {Machine}. TraceId: {TraceId}",
                Environment.MachineName, Activity.Current?.TraceId);

            var problem = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = exception?.Message,
                Extensions = { ["traceId"] = Activity.Current?.TraceId.ToString() }
            };

            var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
            {
                problem.Detail = exception?.ToString();
            }

            await Results.Problem(problem).ExecuteAsync(context);
        });
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();
        try
        {
            stopWatch.Start();
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred on machine {Machine}. TraceId: {TraceId}",
                Environment.MachineName,
                Activity.Current?.TraceId);

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred",
                Extensions = { ["traceId"] = Activity.Current?.TraceId.ToString() },
                Detail = ex.Message
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation("Request processed in {ElapsedMilliseconds} ms", stopWatch.ElapsedMilliseconds);
        }
    }
}
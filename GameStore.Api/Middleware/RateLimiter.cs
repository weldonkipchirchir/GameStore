using System.Threading.RateLimiting;

namespace GameStore.Api.Dtos.Middleware;

public static class RateLimiter
{
    public static IServiceCollection RateLimiterMiddleware(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }
                    ));
            }
        );

        return services;
    }
}
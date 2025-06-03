
namespace GameStore.Api.cors
{
    public static class CorsExtensions
    {
        private const string allowedOriginSetting = "AllowedOrigin";

        public static IServiceCollection AddGameCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(corsBuilder =>
                {
                    var allowedOrigin = configuration[allowedOriginSetting] ?? throw new InvalidOperationException("AllowedOrigin is not set");
                    corsBuilder.WithOrigins(allowedOrigin)
                    .AllowAnyHeader()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination");
                });
            }
        );
            return services;
        }
    }
}
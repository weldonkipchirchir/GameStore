using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public static class DataExtensions
    {
        public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            await context.Database.MigrateAsync();

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Db Initializer");
            logger.LogInformation(5, "Database is ready");

        }

        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration config)
        {
            var connString = config.GetConnectionString("GameStoreContext");

            services.AddDbContext<GameStoreContext>(options => options.UseNpgsql(connString))
                .AddScoped<IGameRepository, EntityFrameworkGameRepository>();

            return services;
        }
    }
}
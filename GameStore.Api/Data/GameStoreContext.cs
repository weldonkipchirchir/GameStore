using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options)
        : base(options)
    {
    }

    // Parameterless constructor for design-time support
    public GameStoreContext() { }

    public DbSet<Game> Games => Set<Game>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = GetConnectionString();
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    private static string GetConnectionString()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddUserSecrets<GameStoreContext>() // Use user secrets in development
            .Build();

        return config.GetConnectionString("GameStoreContext") 
               ?? throw new InvalidOperationException("Database connection string is missing.");
    }
}

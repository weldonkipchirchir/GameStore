using Microsoft.EntityFrameworkCore;
using GameStore.Api.Data;
using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class EntityFrameworkGameRepository : IGameRepository
{
    private readonly GameStoreContext dbContext;
    private readonly ILogger<EntityFrameworkGameRepository> logger;

    public EntityFrameworkGameRepository(GameStoreContext dbContext, ILogger<EntityFrameworkGameRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Game?>> GetGamesAsync()
    {
        return await dbContext.Games.AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetGameAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    public async Task CreateGameAsync(Game game)
    {
        game.ReleaseDate = DateTime.SpecifyKind(game.ReleaseDate, DateTimeKind.Utc);
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Game with id {Id} was created", game.Id);
    }

    public async Task UpdateGameAsync(int id, Game updatedGame)
    {
        dbContext.Games.Update(updatedGame);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteGameAsync(int id)
    {
        dbContext.Games.Where(game => game.Id == id).ExecuteDelete();
        await dbContext.SaveChangesAsync();
    }
}
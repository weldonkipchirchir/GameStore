using Microsoft.EntityFrameworkCore;
using GameStore.Api.Data;
using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class EntityFrameworkGameRepository(GameStoreContext dbContext) : IGameRepository
{
    private readonly GameStoreContext _context = dbContext;

    public async Task<IEnumerable<Game?>> GetGamesAsync()
    {
        throw new InvalidCastException("The database connection is closed.");
        return await _context.Games.AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetGameAsync(int id)
    {
        throw new InvalidCastException("The database connection is closed.");
        return await _context.Games.FindAsync(id);
    }

    public async Task CreateGameAsync(Game game)
    {
        game.ReleaseDate = DateTime.SpecifyKind(game.ReleaseDate, DateTimeKind.Utc);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGameAsync(int id, Game updatedGame)
    {
        _context.Games.Update(updatedGame);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGameAsync(int id)
    {
        _context.Games.Where(game => game.Id == id).ExecuteDelete();
        await _context.SaveChangesAsync();
    }
}
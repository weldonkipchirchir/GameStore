using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game?>> GetGamesAsync(int pageNumber, int pageSize, string? filter);
    Task<Game?> GetGameAsync(int id);
    Task CreateGameAsync(Game game);
    Task UpdateGameAsync(int id, Game updatedGame);
    Task DeleteGameAsync(int id);
    Task<int> CountAsync(string? filter);
}
using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGameRepository
{
    IEnumerable<Game?> GetGames();
    Game? GetGame(int id);
    void CreateGame(Game game);
    void UpdateGame(int id, Game updatedGame);
    void DeleteGame(int id);
}

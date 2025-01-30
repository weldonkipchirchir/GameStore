using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGameRepository : IGameRepository
{
    private readonly List<Game?> games =
  [
      new()
    {
        Id = 1, Name = "Street Fighter II", Genre = "Fighting", Price = 19.99M,
        ReleaseDate = new(1991, 02, 01, 0, 0, 0, DateTimeKind.Utc), ImageUrl = "https://placehold.co/100"
    },
    new()
    {
        Id = 2, Name = "Fantasy XIV", Genre = "RolePlaying", Price = 59.99M,
        ReleaseDate = new(2010, 09, 30, 0, 0, 0, DateTimeKind.Utc), ImageUrl = "https://placehold.co/100"
    },
    new()
    {
        Id = 3, Name = "FIFA 23", Genre = "Sports", Price = 69.99M,
        ReleaseDate = new(2022, 09, 27, 0, 0, 0, DateTimeKind.Utc), ImageUrl = "https://placehold.co/100"
    }
  ];


    public Task<IEnumerable<Game?>> GetGamesAsync() => Task.FromResult(games.AsEnumerable());

    public Task<Game?> GetGameAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        return Task.FromResult(games.Find(game => game != null && game.Id == id));
    }

    public Task CreateGameAsync(Game game)
    {
        game.Id = games.Max(game1 => game1?.Id ?? 0) + 1;
        games.Add(game);
        return Task.CompletedTask;
    }

    public Task UpdateGameAsync(int id, Game updatedGame)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        var existingGame = games.Find(game => game != null && game.Id == id);
        if (existingGame is null) return Task.CompletedTask;

        existingGame.Name = updatedGame.Name;
        existingGame.Genre = updatedGame.Genre;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;
        existingGame.Price = updatedGame.Price;
        existingGame.ImageUrl = updatedGame.ImageUrl;

        return Task.CompletedTask;
    }

    public Task DeleteGameAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        var existingGame = games.FindIndex(game => game != null && game.Id == id);
        if (existingGame == -1) return Task.CompletedTask;

        games.RemoveAt(existingGame);
        return Task.CompletedTask;
    }
}

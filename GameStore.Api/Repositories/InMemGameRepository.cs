using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGameRepository : IGameRepository
{
    private readonly List<Game?> games =
  [
      new()
    {
        Id = 1, Name = "Street Fighter II", Genre = "Fighting", Price = 19.99M,
        ReleaseDate = new(1991, 02, 01, 0, 0, 0, DateTimeKind.Utc), ImageUri = "https://placehold.co/100"
    },
    new()
    {
        Id = 2, Name = "Fantasy XIV", Genre = "RolePlaying", Price = 59.99M,
        ReleaseDate = new(2010, 09, 30, 0, 0, 0, DateTimeKind.Utc), ImageUri = "https://placehold.co/100"
    },
    new()
    {
        Id = 3, Name = "FIFA 23", Genre = "Sports", Price = 69.99M,
        ReleaseDate = new(2022, 09, 27, 0, 0, 0, DateTimeKind.Utc), ImageUri = "https://placehold.co/100"
        }
  ];


    public Task<IEnumerable<Game?>> GetGamesAsync(int pageNumber, int pageSize, string? filter)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pageNumber);
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize);

        var filteredGames = FilterGames(filter)
            .OrderBy(game => game?.Id)
            .AsEnumerable();

        if (pageNumber <= 0 || pageSize <= 0)
        {
            return Task.FromResult(filteredGames);
        }

        return GetPaginatedGamesAsync(filteredGames, pageNumber, pageSize);
    }
    private Task<IEnumerable<Game?>> GetPaginatedGamesAsync(IEnumerable<Game?> games, int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException("Page number and page size must be greater than zero.");
        }

        var paginatedGames = games
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsEnumerable();

        return Task.FromResult(paginatedGames);
    }
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
        existingGame.ImageUri = updatedGame.ImageUri;

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

    public Task<int> CountAsync(string? filter)
    {
        {
            return Task.FromResult(games.Count(game => game != null));
        }
    }

    private IEnumerable<Game> FilterGames(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return games;
        }

        return games.Where(game => game != null && (game.Name.Contains(filter) || game.Genre.Contains(filter)));
    }

}

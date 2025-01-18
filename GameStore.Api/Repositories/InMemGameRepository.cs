using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGameRepository : IGameRepository
{
    private readonly List<Game?> games =
    [
        new Game()
        {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 02, 01, 0, 0, 0, DateTimeKind.Utc),
            ImageUrl = "https://placehold.co/100"
        },

        new Game()
        {
            Id = 2,
            Name = "Final Fantasy XIV",
            Genre = "RolePlaying",
            Price = 59.99M,
            ReleaseDate = new DateTime(2010, 09, 30, 0, 0, 0, DateTimeKind.Utc),
            ImageUrl = "https://placehold.co/100"
        },

        new Game()
        {
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateTime(2022, 09, 27, 0, 0, 0, DateTimeKind.Utc),
            ImageUrl = "https://placehold.co/100"
        }
    ];

    public IEnumerable<Game?> GetGames() => games;

    public Game? GetGame(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        return games.Find(game => game != null && game.Id == id);
    }

    public void CreateGame(Game game)
    {
        game.Id = games.Max(game1 => game1?.Id ?? 0) + 1;
        games.Add(game);
    }

    public void UpdateGame(int id, Game updatedGame)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);

        var existingGame = games.Find(game => game != null && game.Id == id);
        if (existingGame is null) return;

        existingGame.Name = updatedGame.Name;
        existingGame.Genre = updatedGame.Genre;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;
        existingGame.Price = updatedGame.Price;
        existingGame.ImageUrl = updatedGame.ImageUrl;
    }

    public void DeleteGame(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);

        var existingGame = games.FindIndex(game => game != null && game.Id == id);
        if (existingGame == -1) return;
        games.RemoveAt(existingGame);
    }
}
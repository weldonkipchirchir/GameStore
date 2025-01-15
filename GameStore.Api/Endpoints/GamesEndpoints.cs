using GameStore.Api.Entities;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    
    const string GetGameEndpoint = "GetGame";

    private static List<Game> games =
    [
        new Game()
        {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 02, 01),
            ImageUrl = "https://placehold.co/100"
        },

        new Game()
        {
            Id = 2,
            Name = "Final Fantasy XIV",
            Genre = "RolePlaying",
            Price = 59.99M,
            ReleaseDate = new DateTime(2010, 09, 30),
            ImageUrl = "https://placehold.co/100"
        },

        new Game()
        {
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateTime(2022, 09, 27),
            ImageUrl = "https://placehold.co/100"
        }
    ];
    
    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        
        var group = routes.MapGroup("/games")
            .WithParameterValidation();

        routes.MapGet("/", () => "!");

        group.MapGet("/", () => games);

        group.MapGet("/{id:int}", (int id) =>
            {
                var game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpoint);

        group.MapPost("/", (Game game) =>
        {
            game.Id = games.Max(game1 => game1.Id) + 1;
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game);
        });

        group.MapPut("/{id:int}", (int id, Game updatedGame) =>
        { 
            var existingGame = games.Find(game => game.Id == id);

            if (existingGame is null) return Results.NotFound();

            existingGame.Name = updatedGame.Name;
            existingGame.Genre = updatedGame.Genre;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;
            existingGame.Price = updatedGame.Price;
            existingGame.ImageUrl = updatedGame.ImageUrl;

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id) =>
        {
            var existingGame = games.Find(game => game.Id == id);
            if (existingGame is null) return Results.NotFound();
            games.Remove(existingGame);

            return Results.NoContent();
        });
        
        return group;
    }
}
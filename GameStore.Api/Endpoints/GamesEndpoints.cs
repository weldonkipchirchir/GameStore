using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{

    const string GetGameEndpoint = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {


        var group = routes.MapGroup("/games")
            .WithParameterValidation();


        group.MapGet("/", (IGameRepository repository) => repository.GetGames().Select(game => game.AsDto()));

        group.MapGet("/{id:int}", (int id, IGameRepository repository) =>
            {
                var game = repository.GetGame(id);

                return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
            })
            .WithName(GetGameEndpoint);

        group.MapPost("/", (CreateGameDto gameDto, IGameRepository repository) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUrl = gameDto.ImageUrl
            };

            repository.CreateGame(game);

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game.AsDto());
        });

        group.MapPut("/{id:int}", (int id, UpdateGameDto updatedGameDto, IGameRepository repository) =>
        {
            var game = repository.GetGame(id);

            if (game is null) return Results.NotFound();

            game.Name = updatedGameDto.Name;
            game.Genre = updatedGameDto.Genre;
            game.Price = updatedGameDto.Price;
            game.ReleaseDate = updatedGameDto.ReleaseDate;
            game.ImageUrl = updatedGameDto.ImageUrl;

            if (game is null) return Results.NotFound();

            repository.UpdateGame(id, game);

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IGameRepository repository) =>
        {
            var game = repository.GetGame(id);

            if (game is null) return Results.NotFound();

            repository.DeleteGame(id);

            return Results.NoContent();
        });

        return group;
    }
}
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameEndpoint = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games")
                          .WithParameterValidation();

        group.MapGet("/", async (IGameRepository repository) =>
        {
            var games = await repository.GetGamesAsync();
            return games.Select(game => game?.AsDto());
        });

        group.MapGet("/{id:int}", async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
        })
        .WithName(GetGameEndpoint);

        group.MapPost("/", async (CreateGameDto gameDto, IGameRepository repository) =>
        {
            var game = new Game
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUrl = gameDto.ImageUrl
            };

            await repository.CreateGameAsync(game);

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game.AsDto());
        });

        group.MapPut("/{id:int}", async (int id, UpdateGameDto updatedGameDto, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            if (game is null) return Results.NotFound();

            game.Name = updatedGameDto.Name;
            game.Genre = updatedGameDto.Genre;
            game.Price = updatedGameDto.Price;
            game.ReleaseDate = updatedGameDto.ReleaseDate;
            game.ImageUrl = updatedGameDto.ImageUrl;

            await repository.UpdateGameAsync(id, game);

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            if (game is null) return Results.NotFound();

            await repository.DeleteGameAsync(id);

            return Results.NoContent();
        });

        return group;
    }

    private static async Task<IResult> GetGameByIdAsync(int id, IGameRepository repository)
    {
        var game = await repository.GetGameAsync(id);
        return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
    }
}

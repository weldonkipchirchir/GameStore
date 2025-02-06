using System.Diagnostics;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Authorization;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameEndpoint = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games")
                          .WithParameterValidation();

        // Get all games
        group.MapGet("/", async (IGameRepository repository, ILoggerFactory loggerFactory) =>
        {
                var games = await repository.GetGamesAsync();
                return Results.Ok(games.Select(game => game?.AsDto()));
        });

        // Get game by ID
        group.MapGet("/{id:int}", async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
        })
        .WithName(GetGameEndpoint).RequireAuthorization(Policies.ReadAccess);

        // Create a new game
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
        }).RequireAuthorization(Policies.WriteAccess);

        // Update an existing game
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
        }).RequireAuthorization(Policies.WriteAccess);

        // Delete a game
        group.MapDelete("/{id:int}", async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            if (game is null) return Results.NotFound();

            await repository.DeleteGameAsync(id);

            return Results.NoContent();
        }).RequireAuthorization(Policies.WriteAccess);

        return group;
    }
}
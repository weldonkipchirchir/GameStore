using System.Diagnostics;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Authorization;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameV1Endpoint = "GetGameV1";
    private const string GetGameV2Endpoint = "GetGameV2";
    private const string GameByIdRoute = "/{id:int}";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi()
                    .MapGroup("/games")
                    .HasApiVersion(1.0)
                    .HasApiVersion(2.0)
                    .WithParameterValidation();

        // version 1 endpoints
        // Get all games
        group.MapGet("/", async (IGameRepository repository, ILoggerFactory loggerFactory, [AsParameters] GetGameDtov1 request, HttpContext httpContext) =>
        {
            var totalCount = await repository.CountAsync(null);
            httpContext.Response.AddPaginationHeader(request.PageSize, totalCount);

            var games = await repository.GetGamesAsync(request.PageNumber, request.PageSize, null);
            return Results.Ok(games.Select(game => game?.AsDtoV1()));
        }).MapToApiVersion(1.0);

        group.MapGet(GameByIdRoute, async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDtoV1());
        })
        .WithName(GetGameV1Endpoint).RequireAuthorization(Policies.ReadAccess).MapToApiVersion(1.0);

        // version 2 endpoints
        group.MapGet("/", async (IGameRepository repository, ILoggerFactory loggerFactory, [AsParameters] GetGameDtov2 request, HttpContext httpContext) =>
       {
           var totalCount = await repository.CountAsync(null);
           httpContext.Response.AddPaginationHeader(request.PageSize, totalCount);

           var games = await repository.GetGamesAsync(request.PageNumber, request.PageSize, null);
           return Results.Ok(games.Select(game => game?.AsDtoV2()));
       }).MapToApiVersion(2.0);

        // Get game by ID
        group.MapGet(GameByIdRoute, async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.AsDtoV2());
        })
        .WithName(GetGameV2Endpoint).RequireAuthorization(Policies.ReadAccess).MapToApiVersion(2.0);


        // Create a new game
        group.MapPost("/", async (CreateGameDto gameDto, IGameRepository repository) =>
        {
            var game = new Game
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUri = gameDto.ImageUri
            };

            await repository.CreateGameAsync(game);

            return Results.CreatedAtRoute(GetGameV1Endpoint, new { id = game.Id }, game.AsDtoV1());
        }).RequireAuthorization(Policies.WriteAccess).MapToApiVersion(1.0);

        group.MapPut(GameByIdRoute, async (int id, UpdateGameDto updatedGameDto, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            if (game is null) return Results.NotFound();

            game.Name = updatedGameDto.Name;
            game.Genre = updatedGameDto.Genre;
            game.Price = updatedGameDto.Price;
            game.ReleaseDate = updatedGameDto.ReleaseDate;
            game.ImageUri = updatedGameDto.ImageUri;

            await repository.UpdateGameAsync(id, game);

            return Results.NoContent();
        }).RequireAuthorization(Policies.WriteAccess).MapToApiVersion(1.0);

        group.MapDelete(GameByIdRoute, async (int id, IGameRepository repository) =>
        {
            var game = await repository.GetGameAsync(id);
            if (game is null) return Results.NotFound();

            await repository.DeleteGameAsync(id);

            return Results.NoContent();
        }).RequireAuthorization(Policies.WriteAccess).MapToApiVersion(1.0);

        return group;
    }
}
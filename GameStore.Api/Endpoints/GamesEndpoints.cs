using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    
    const string GetGameEndpoint = "GetGame";
    
    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
    InMemGameRepository repository = new();
        
        var group = routes.MapGroup("/games")
            .WithParameterValidation();

        routes.MapGet("/", () => "!");

        group.MapGet("/", () => repository.GetGames());

        group.MapGet("/{id:int}", (int id) =>
            {
                var game = repository.GetGame(id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpoint);
  
        group.MapPost("/", (Game game) =>
        {
            repository.CreateGame(game);
            return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game);
        });

        group.MapPut("/{id:int}", (int id, Game updatedGame) =>
        { 
            var game = repository.GetGame(id);
            
            if (game is null) return Results.NotFound();
            
            repository.UpdateGame(id, updatedGame);

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id) =>
        {
            var game = repository.GetGame(id);
            
            if (game is null) return Results.NotFound();
            
            repository.DeleteGame(id);

            return Results.NoContent();
        });
        
        return group;
    }
}
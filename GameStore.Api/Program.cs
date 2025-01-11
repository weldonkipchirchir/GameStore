using GameStore.Api.Entities;

const string GetGameEndpoint = "GetGame";

List<Game> games =
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

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "!");

app.MapGet("/games", () => games);

app.MapGet("/games/{id:int}", (int id) =>
{
    var game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpoint);

app.MapPost("/games", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game);
});
await app.RunAsync();

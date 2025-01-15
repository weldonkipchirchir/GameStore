using GameStore.Api.Endpoints;
using GameStore.Api.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGamesEndpoints();

await app.RunAsync();

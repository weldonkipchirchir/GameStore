using GameStore.Api.Endpoints;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGameRepository, InMemGameRepository>();

var app = builder.Build();

app.MapGamesEndpoints();

await app.RunAsync();

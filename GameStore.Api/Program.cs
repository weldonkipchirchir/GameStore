using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositoryServices(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

app.MapGamesEndpoints();

await app.RunAsync();
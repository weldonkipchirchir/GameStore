using System.Text;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Entities;
using GameStore.Api.Authorization;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add repository services
builder.Services.AddRepositoryServices(builder.Configuration);

// Add JWT authentication
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddAuthorizationPolicies();    

// Build the application
var app = builder.Build();

// Add middleware
app.UseAuthentication();

app.UseAuthorization();

// Initialize database and map endpoints
await app.Services.InitializeDatabaseAsync();

app.MapGamesEndpoints();

await app.RunAsync();

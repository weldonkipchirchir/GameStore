using System.Diagnostics;
using System.Text;
using System.Threading.RateLimiting;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Entities;
using GameStore.Api.Authorization;
using GameStore.Api.Dtos.ErrorHandling;
using GameStore.Api.Dtos.Middleware;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add repository services
builder.Services.AddRepositoryServices(builder.Configuration);

// Add JWT authentication
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddAuthorizationPolicies();

builder.Services.RateLimiterMiddleware();

//builder.Services.AddTransient<ExceptionsMiddleware>();
builder.Services.AddHttpLogging(options =>
{
    // Optional: Configure logging options here
});

// Build the application
var app = builder.Build();

// Add middleware
app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandling());

//app.UseMiddleware<ExceptionsMiddleware>();

// Initialize database and map endpoints
await app.Services.InitializeDatabaseAsync();

app.UseHttpLogging();

app.MapGamesEndpoints();

await app.RunAsync();

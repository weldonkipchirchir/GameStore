using System.Text;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Entities;
using GameStore.Api.jwt;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add repository services
builder.Services.AddRepositoryServices(builder.Configuration);

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings are not properly configured in appsettings.json");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudiences = new[] { "http://localhost:5080", "https://localhost:7073" },
        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(jwtSettings.Key)
        ),
        RequireSignedTokens = true,
        RequireExpirationTime = true
    };
    
    // Add these debug options to get more information
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Build the application
var app = builder.Build();

// Add middleware
Console.WriteLine("Adding authentication middleware...");
app.UseAuthentication();

Console.WriteLine("Adding authorization middleware...");
app.UseAuthorization();

// Initialize database and map endpoints
await app.Services.InitializeDatabaseAsync();

Console.WriteLine("Mapping endpoints...");
app.MapGamesEndpoints();

Console.WriteLine("Starting application...");
await app.RunAsync();




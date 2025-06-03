using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GameStore.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services
            .AddScoped<IClaimsTransformation, ScopeTransformation>()    
            .AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ReadAccess, builder =>
                builder.RequireClaim("scope", "games:read")
                    .AddAuthenticationSchemes("Auth0"));
            
            options.AddPolicy(Policies.WriteAccess, policy =>
                policy.RequireClaim("scope", "games:write").RequireRole("Admin")
                .AddAuthenticationSchemes("Auth0"));
        });

        return services;
    }

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are not properly configured in appsettings.json");

        var auth0Settings = config.GetSection("Authentication:Schemes:Auth0").Get<Auth0Settings>()
                            ?? throw new InvalidOperationException("Auth0 settings are not properly configured in appsettings.json");
        
        services.AddAuthentication(options =>
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
        })
        .AddJwtBearer("Auth0", options =>
        {
            options.Authority = auth0Settings.Authority;
            options.Audience = auth0Settings.ValidAudiences?.FirstOrDefault() ?? "https://gamestore";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false
            };
        });

        return services;
    }
}
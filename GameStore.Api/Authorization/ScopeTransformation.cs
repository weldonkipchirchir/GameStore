using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace GameStore.Api.Authorization;

public class ScopeTransformation : IClaimsTransformation
{
    private const string ScopeClaimType = "scope";
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var scopeClaim = principal.FindFirst(ScopeClaimType);

        if (scopeClaim == null)
        {
            return Task.FromResult(principal);
        }
        var scopes = scopeClaim.Value.Split(' ');
        
        var originalIdentity = (ClaimsIdentity)principal.Identity!;
        
        var identity = new ClaimsIdentity(originalIdentity);
        
        var originalClaims = identity.Claims.FirstOrDefault(claim => claim.Type == ScopeClaimType);
        if (originalClaims != null)
        {
            identity.RemoveClaim(originalClaims);
        }
        
        identity.AddClaims(scopes.Select(scope => new Claim(ScopeClaimType, scope)));
        
        return Task.FromResult(new ClaimsPrincipal(identity));
    }
}
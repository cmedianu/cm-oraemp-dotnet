using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using OraEmp.Application.Services;

namespace OraEmp.Infrastructure.Identity;

public class EmpAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IdentityInformation _identity;

    public EmpAuthenticationStateProvider(IdentityInformation identity)
    {
        _identity = identity;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return await Task.FromResult(new AuthenticationState(GetClaimsPrincipal()));
    }

    private bool Authenticated { get; set; } = false;

    private ClaimsPrincipal GetClaimsPrincipal()
    {
        List<Claim> claims = new();
        ClaimsIdentity identity;

        string? authentication;
        if (Authenticated)
        {
            authentication = "HARDCODED";
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "cmedianu"));
            claims.Add(new Claim(ClaimTypes.Name, "Calin Medianu"));
            claims.Add(new Claim(ClaimTypes.Role, "USER"));
            identity = new ClaimsIdentity(claims, authentication);
        }
        else
        {
            authentication = null;
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "NOBODY"));
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "NOBODY")
            }, authentication);
        }
        var user = new ClaimsPrincipal(identity);
        return user;
    }
}

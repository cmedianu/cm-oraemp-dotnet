using System.Collections;
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
            authentication = "TEST";
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _identity.State.LoggedInUser));
            foreach (var role in _identity.State.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
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

    public async Task<bool> Login(string loginUser)
    {
        Authenticated = true;
        Hashtable roles = new();
        roles.Add("calin",new [] {"USER","ADMIN"});
        roles.Add("admin", new[] {"ADMIN"});
        roles.Add("user", new[] {"USER"});
        _identity.State.setLoggedInUser(loginUser, (string[]) roles[loginUser]!);
        // NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return true;
    }

}

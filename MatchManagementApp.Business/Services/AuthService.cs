using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

public class AuthService : IAuthService
{
    public ClaimsPrincipal GetClaimsPrincipal(UserDto user)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) 
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}

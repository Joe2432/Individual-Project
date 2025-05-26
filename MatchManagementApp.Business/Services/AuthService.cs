using System.Security.Claims;

public class AuthService : IAuthService
{
    public ClaimsPrincipal GetClaimsPrincipal(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "MatchAppAuth");
        return new ClaimsPrincipal(identity);
    }
}

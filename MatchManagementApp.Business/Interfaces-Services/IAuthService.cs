using System.Security.Claims;

public interface IAuthService
{
    ClaimsPrincipal GetClaimsPrincipal(UserDto user);
}

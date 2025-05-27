using System.Security.Claims;

public interface IAuthService
{
    ClaimsPrincipal GetClaimsPrincipal(UserCreateDto user);
}

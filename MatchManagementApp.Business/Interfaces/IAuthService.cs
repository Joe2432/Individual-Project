using System.Security.Claims;

public interface IAuthService
{
    ClaimsPrincipal GetClaimsPrincipal(UserEntity user);
}

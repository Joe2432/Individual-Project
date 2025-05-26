using System.Security.Claims;

public interface IPointService
{
    Task RegisterPointAsync(PointCreateDto dto, ClaimsPrincipal user);
    Task<List<PointReadDto>> GetPointsForMatchAsync(int matchId, ClaimsPrincipal user);
}

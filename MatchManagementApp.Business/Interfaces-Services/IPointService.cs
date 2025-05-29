using System.Security.Claims;

public interface IPointService
{
    Task RegisterPointAsync(PointDto dto, ClaimsPrincipal user);
    Task<List<PointDto>> GetPointsForMatchAsync(int matchId, ClaimsPrincipal user);
}

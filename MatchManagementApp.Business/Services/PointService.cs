using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PointService : IPointService
{
    private readonly IPointRepository _pointRepository;
    private readonly IUserService _userService;

    public PointService(IPointRepository pointRepository, IUserService userService)
    {
        _pointRepository = pointRepository;
        _userService = userService;
    }

    public async Task RegisterPointAsync(PointCreateDto dto, ClaimsPrincipal user)
    {
        var userId = await _userService.GetCurrentUserId(user)
            ?? throw new InvalidOperationException("User not logged in.");

        // Map to "User" or "Opponent" label
        var winnerLabel = dto.IsUserWinner ? "User" : "Opponent";
        var point = new PointEntity(dto.MatchId, winnerLabel, dto.PointType, dto.NumberOfShots);

        await _pointRepository.AddPointAsync(point);
    }

    public async Task<List<PointReadDto>> GetPointsForMatchAsync(int matchId, ClaimsPrincipal user)
    {
        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        return points.Select(p => p.ToReadDto()).ToList(); // No userId needed anymore
    }
}

using System.Security.Claims;

public class PointService : IPointService
{
    private readonly IPointRepository _pointRepository;
    private readonly IUserService _userService;

    public PointService(IPointRepository pointRepository, IUserService userService)
    {
        _pointRepository = pointRepository;
        _userService = userService;
    }

    public async Task RegisterPointAsync(PointDto dto, ClaimsPrincipal user)
    {
        var userId = await _userService.GetCurrentUserIdAsync(user);

        await _pointRepository.AddPointAsync(dto);
    }

    public async Task<List<PointDto>> GetPointsForMatchAsync(int matchId, ClaimsPrincipal user)
    {
        return await _pointRepository.GetPointsByMatchIdAsync(matchId);
    }

}

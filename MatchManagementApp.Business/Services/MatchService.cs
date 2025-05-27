public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPointRepository _pointRepository;

    public MatchService(IMatchRepository matchRepository, IPointRepository pointRepository)
    {
        _matchRepository = matchRepository;
        _pointRepository = pointRepository;
    }

    public async Task<int> CreateMatchAsync(MatchCreateDto dto)
    {
        var entity = MatchMapper.ToEntity(dto);
        return await _matchRepository.CreateMatchAsync(entity); // Correct method name
    }



    public async Task<List<MatchCreateDto>> GetUserMatchesAsync(int userId)
    {
        var matches = await _matchRepository.GetMatchesByUserIdAsync(userId);
        return matches.Select(m => m.ToDto()).ToList();
    }

    public async Task<string> GetScoreDisplayAsync(int matchId, int userId)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null) return "Match not found";

        var points = match.Points;
        var player1Points = points.Count(p => p.WinnerLabel == "User");
        var player2Points = points.Count(p => p.WinnerLabel == "Opponent");

        return $"{player1Points} - {player2Points}";
    }

    public async Task RegisterPointAsync(int matchId, int userId, string pointType, bool isUserWinner)
    {
        var winnerLabel = isUserWinner ? "User" : "Opponent";

        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null) return;

        var point = new PointEntity(matchId, winnerLabel, pointType, 0); // You can pass shot count if needed
        await _pointRepository.AddPointAsync(point);
    }


    public async Task UndoLastPointAsync(int matchId)
    {
        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var lastPoint = points.OrderByDescending(p => p.Id).FirstOrDefault();
        if (lastPoint != null)
        {
            await _pointRepository.DeletePointAsync(lastPoint);
        }
    }
}
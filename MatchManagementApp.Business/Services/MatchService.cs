public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPointRepository _pointRepository;

    public MatchService(IMatchRepository matchRepository, IPointRepository pointRepository)
    {
        _matchRepository = matchRepository;
        _pointRepository = pointRepository;
    }

    public async Task<int> CreateMatchAsync(MatchDto matchDto)
    {
        var model = MatchDtoMapper.ToModel(matchDto);
        var id = await _matchRepository.CreateMatchAsync(matchDto);
        model.SetId(id);
        return id;
    }

    public async Task DeleteMatchAsync(int matchId)
    {
        await _matchRepository.DeleteMatchAsync(matchId);
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        var dto = await _matchRepository.GetMatchByIdAsync(matchId);
        if (dto is null) return null;
        return dto;
    }

    public async Task<List<MatchDto>> GetUserMatchesAsync(int userId)
    {
        var matches = await _matchRepository.GetMatchesByUserAsync(userId);
        return matches;
    }

    public async Task<PlayMatchDto?> GetPlayMatchDtoAsync(int matchId, int userId)
    {
        var matchDto = await _matchRepository.GetMatchByIdAsync(matchId);
        if (matchDto == null || matchDto.CreatedByUserId != userId) return null;

        var pointDtos = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var model = MatchDtoMapper.ToModel(matchDto);
        foreach (var pt in pointDtos)
            model.AddPoint(PointDtoMapper.ToModel(pt));

        var score = model.CalculateScore();
        return new PlayMatchDto
        {
            MatchId = matchDto.Id,
            MatchType = matchDto.MatchType,
            PartnerName = matchDto.PartnerName,
            Opponent1 = matchDto.FirstOpponentName,
            Opponent2 = matchDto.SecondOpponentName,
            Surface = matchDto.Surface,
            Score = model.GetScoreDisplay(),
            Status = model.IsOver() ? "Finished" : "In Progress"
        };
    }

    public async Task<string> GetScoreDisplayAsync(int matchId, int userId)
    {
        var matchDto = await _matchRepository.GetMatchByIdAsync(matchId);
        if (matchDto == null || matchDto.CreatedByUserId != userId)
            throw new UnauthorizedAccessException();

        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var model = MatchDtoMapper.ToModel(matchDto);
        foreach (var p in points)
            model.AddPoint(PointDtoMapper.ToModel(p));

        return model.GetScoreDisplay();
    }

    public async Task RegisterPointAsync(int matchId, int userId, string winnerLabel, bool isServe)
    {
        var matchDto = await _matchRepository.GetMatchByIdAsync(matchId);
        if (matchDto == null || matchDto.CreatedByUserId != userId) throw new UnauthorizedAccessException();

        var isUserWinner = winnerLabel == "User";
        var point = new Point(matchId, winnerLabel, userId, isServe);
        await _pointRepository.AddPointAsync(PointDtoMapper.ToDto(point));
    }

    public async Task UndoLastPointAsync(int matchId)
    {
        await _pointRepository.RemoveLastPointAsync(matchId);
    }

    public async Task<List<MatchHistoryDto>> GetMatchHistorySummariesAsync(int userId, string? type, string? surface, string? name, DateTime? start, DateTime? end, DateTime? today)
    {
        var matches = await _matchRepository.SearchMatchesAsync(userId, type, surface, name, start, end, today);
        return matches.Select(m => MatchHistoryDtoMapper.FromDto(m)).ToList();
    }
}

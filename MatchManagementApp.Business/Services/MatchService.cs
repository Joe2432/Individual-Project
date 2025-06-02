public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPointRepository _pointRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScorekeepingService _scorekeepingService;

    public MatchService(
        IMatchRepository matchRepository,
        IPointRepository pointRepository,
        IUserRepository userRepository,
        IScorekeepingService scorekeepingService)
    {
        _matchRepository = matchRepository;
        _pointRepository = pointRepository;
        _userRepository = userRepository;
        _scorekeepingService = scorekeepingService;
    }

    public async Task<int> CreateMatchAsync(MatchDto matchDto)
    {
        var user = await _userRepository.GetUserByIdAsync(matchDto.CreatedByUserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var matchId = await _matchRepository.CreateMatchAsync(matchDto);
        matchDto.Id = matchId;
        return matchId;
    }

    public async Task DeleteMatchAsync(int matchId)
    {
        await _matchRepository.DeleteMatchAsync(matchId);
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        return await _matchRepository.GetMatchByIdAsync(matchId);
    }

    public async Task<List<MatchDto>> GetUserMatchesAsync(int userId)
    {
        var matches = await _matchRepository.GetMatchesByUserIdAsync(userId);

        foreach (var match in matches)
        {
            var points = await _pointRepository.GetPointsByMatchIdAsync(match.Id);
            var updated = _scorekeepingService.CalculateScore(match, points);

            match.SetScores = updated.SetScores;
            match.ScoreSummary = updated.ScoreSummary;
            match.MatchOver = updated.MatchOver;
            match.CurrentGameScore = updated.CurrentGameScore;
            match.InTiebreak = updated.InTiebreak;
            match.DisplaySetIndices = updated.DisplaySetIndices;
            match.UserSetGames = updated.UserSetGames;
            match.OpponentSetGames = updated.OpponentSetGames;

            var parts = updated.CurrentGameScore.Split('-');
            match.GameUserDisplay = parts.Length == 2 ? parts[0].Trim() : updated.CurrentGameScore;
            match.GameOpponentDisplay = parts.Length == 2 ? parts[1].Trim() : updated.CurrentGameScore;
        }

        return matches;
    }

    public async Task<string> GetScoreDisplayAsync(int matchId, int userId)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
            return "Match not found";

        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var scored = _scorekeepingService.CalculateScore(match, points);

        return scored.CurrentGameScore ?? "0 - 0";
    }

    public async Task RegisterPointAsync(int matchId, int userId, string pointType, bool isUserWinner)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
            return;

        var point = new PointDto
        {
            MatchId = matchId,
            PointType = pointType,
            IsUserWinner = isUserWinner,
            NumberOfShots = 0
        };

        await _pointRepository.AddPointAsync(point);
    }

    public async Task UndoLastPointAsync(int matchId)
    {
        await _pointRepository.DeleteLastPointAsync(matchId);
    }

    public async Task<MatchDto?> GetMatchForPlayingAsync(int matchId, int userId)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
            return null;

        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var updated = _scorekeepingService.CalculateScore(match, points);

        match.SetScores = updated.SetScores;
        match.ScoreSummary = updated.ScoreSummary;
        match.MatchOver = updated.MatchOver;
        match.CurrentGameScore = updated.CurrentGameScore;
        match.InTiebreak = updated.InTiebreak;
        match.DisplaySetIndices = updated.DisplaySetIndices;
        match.UserSetGames = updated.UserSetGames;
        match.OpponentSetGames = updated.UserSetGames;

        var parts = updated.CurrentGameScore.Split('-');
        match.GameUserDisplay = parts.Length == 2 ? parts[0].Trim() : updated.CurrentGameScore;
        match.GameOpponentDisplay = parts.Length == 2 ? parts[1].Trim() : updated.CurrentGameScore;

        return match;
    }

    public async Task<List<MatchDto>> GetMatchHistorySummariesAsync(
        int userId,
        string? name,
        string? type,
        string? surface,
        DateTime? date,
        DateTime? dateFrom,
        DateTime? dateTo)
    {
        var matches = await _matchRepository.GetMatchesByUserIdAsync(userId);

        if (!string.IsNullOrWhiteSpace(name))
        {
            matches = matches.Where(m =>
                (m.MatchType == "Singles" && m.FirstOpponentName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                (m.MatchType == "Doubles" &&
                 (m.FirstOpponentName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                  m.SecondOpponentName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                  m.PartnerName.Contains(name, StringComparison.OrdinalIgnoreCase)))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(type))
            matches = matches.Where(m => m.MatchType.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(surface))
            matches = matches.Where(m => m.Surface.Equals(surface, StringComparison.OrdinalIgnoreCase)).ToList();

        if (date.HasValue)
            matches = matches.Where(m => m.MatchDate?.Date == date.Value.Date).ToList();

        if (dateFrom.HasValue)
            matches = matches.Where(m => m.MatchDate >= dateFrom.Value).ToList();

        if (dateTo.HasValue)
            matches = matches.Where(m => m.MatchDate <= dateTo.Value).ToList();

        foreach (var match in matches)
        {
            var points = await _pointRepository.GetPointsByMatchIdAsync(match.Id);
            var updated = _scorekeepingService.CalculateScore(match, points);

            match.SetScores = updated.SetScores;
            match.ScoreSummary = updated.ScoreSummary;
            match.MatchOver = updated.MatchOver;
            match.CurrentGameScore = updated.CurrentGameScore;
            match.InTiebreak = updated.InTiebreak;
            match.DisplaySetIndices = updated.DisplaySetIndices;
            match.UserSetGames = updated.UserSetGames;
            match.OpponentSetGames = updated.UserSetGames;

            var parts = updated.CurrentGameScore.Split('-');
            match.GameUserDisplay = parts.Length == 2 ? parts[0].Trim() : updated.CurrentGameScore;
            match.GameOpponentDisplay = parts.Length == 2 ? parts[1].Trim() : updated.CurrentGameScore;
        }

        return matches;
    }
    public async Task UpdateInitialServerAsync(int matchId, string initialServer)
    {
        await _matchRepository.UpdateInitialServerAsync(matchId, initialServer);
    }

}

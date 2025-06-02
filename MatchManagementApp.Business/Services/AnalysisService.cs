public class AnalysisService : IAnalysisService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPointRepository _pointRepository;
    private readonly IScorekeepingService _scorekeepingService;

    public AnalysisService(
        IMatchRepository matchRepository,
        IPointRepository pointRepository,
        IScorekeepingService scorekeepingService
    )
    {
        _matchRepository = matchRepository;
        _pointRepository = pointRepository;
        _scorekeepingService = scorekeepingService;
    }

    public async Task<AnalysisDto> GetAnalysisAsync(int matchId, int currentUserId)
    {
        var matchEntity = await _matchRepository.GetMatchByIdAsync(matchId);
        if (matchEntity == null || matchEntity.CreatedByUserId != currentUserId)
            throw new InvalidOperationException("Match not found or access denied");

        var matchDto = new MatchDto
        {
            Id = matchEntity.Id,
            CreatedByUserId = matchEntity.CreatedByUserId,
            MatchType = matchEntity.MatchType,
            PartnerName = matchEntity.PartnerName,
            FirstOpponentName = matchEntity.FirstOpponentName,
            SecondOpponentName = matchEntity.SecondOpponentName ?? string.Empty,
            NrSets = matchEntity.NrSets,
            FinalSetType = matchEntity.FinalSetType,
            GameFormat = matchEntity.GameFormat,
            Surface = matchEntity.Surface,
            MatchDate = matchEntity.MatchDate
        };

        var pointDtos = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        matchDto = _scorekeepingService.CalculateScore(matchDto, pointDtos);

        var acesA = pointDtos.Count(p => p.PointType == "Ace" && p.IsUserWinner);
        var acesB = pointDtos.Count(p => p.PointType == "Ace" && !p.IsUserWinner);
        var dfA = pointDtos.Count(p => p.PointType == "Double Fault" && p.IsUserWinner);
        var dfB = pointDtos.Count(p => p.PointType == "Double Fault" && !p.IsUserWinner);
        var wA = pointDtos.Count(p => p.PointType == "Winner" && p.IsUserWinner);
        var wB = pointDtos.Count(p => p.PointType == "Winner" && !p.IsUserWinner);
        var ufeA = pointDtos.Count(p => p.PointType == "Unforced Error" && p.IsUserWinner);
        var ufeB = pointDtos.Count(p => p.PointType == "Unforced Error" && !p.IsUserWinner);
        var feA = pointDtos.Count(p => p.PointType == "Forced Error" && p.IsUserWinner);
        var feB = pointDtos.Count(p => p.PointType == "Forced Error" && !p.IsUserWinner);
        var ptsA = pointDtos.Count(p => p.IsUserWinner);
        var ptsB = pointDtos.Count(p => !p.IsUserWinner);
        var gamesA = matchDto.SetScores.Sum(s => s.Player1Games);
        var gamesB = matchDto.SetScores.Sum(s => s.Player2Games);

        var gamesListA = matchDto.SetScores.Select(s =>
            s.TiebreakScore.HasValue
                ? $"{s.Player1Games}-{s.Player2Games}({s.TiebreakScore})"
                : $"{s.Player1Games}-{s.Player2Games}"
        ).ToList();

        var gamesListB = matchDto.SetScores.Select(s =>
            s.TiebreakScore.HasValue
                ? $"{s.Player1Games}-{s.Player2Games}({s.TiebreakScore})"
                : $"{s.Player1Games}-{s.Player2Games}"
        ).ToList();

        return new AnalysisDto
        {
            MatchId = matchDto.Id,
            PlayerAName = matchDto.FirstOpponentName,
            PlayerBName = matchDto.SecondOpponentName,
            MatchDate = matchDto.MatchDate,
            PlayerAGameByGame = gamesListA,
            PlayerBGameByGame = gamesListB,
            AcesPlayerA = acesA,
            AcesPlayerB = acesB,
            DoubleFaultsPlayerA = dfA,
            DoubleFaultsPlayerB = dfB,
            WinnersPlayerA = wA,
            WinnersPlayerB = wB,
            UnforcedErrorsPlayerA = ufeA,
            UnforcedErrorsPlayerB = ufeB,
            ForcedErrorsPlayerA = feA,
            ForcedErrorsPlayerB = feB,
            TotalPointsPlayerA = ptsA,
            TotalPointsPlayerB = ptsB,
            TotalGamesPlayerA = gamesA,
            TotalGamesPlayerB = gamesB
        };
    }
}

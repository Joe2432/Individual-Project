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
        var matchDto = await _matchRepository.GetMatchByIdAsync(matchId);
        var pointDtos = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        matchDto = _scorekeepingService.CalculateScore(matchDto, pointDtos);

        var acesA = pointDtos.Count(p => p.PointType == "Ace" && p.IsUserWinner);
        var acesB = pointDtos.Count(p => p.PointType == "Ace" && !p.IsUserWinner);
        var dfA = pointDtos.Count(p => p.PointType == "Double Fault" && p.IsUserWinner);
        var dfB = pointDtos.Count(p => p.PointType == "Double Fault" && !p.IsUserWinner);

        var firstServeAttempts = pointDtos.Count(p => p.IsFirstServe);
        var firstServeWonA = pointDtos.Count(p => p.IsFirstServe && p.IsUserWinner);
        var firstServeWonB = pointDtos.Count(p => p.IsFirstServe && !p.IsUserWinner);
        var firstServePctA = firstServeAttempts > 0
            ? Math.Round(firstServeWonA * 100.0 / firstServeAttempts, 0)
            : 0;
        var firstServePctB = firstServeAttempts > 0
            ? Math.Round(firstServeWonB * 100.0 / firstServeAttempts, 0)
            : 0;

        var secondServeAttempts = pointDtos.Count(p => !p.IsFirstServe);
        var secondServeWonA = pointDtos.Count(p => !p.IsFirstServe && p.IsUserWinner);
        var secondServeWonB = pointDtos.Count(p => !p.IsFirstServe && !p.IsUserWinner);
        var winPct1A = (firstServeWonA + firstServeWonB) > 0
            ? Math.Round(firstServeWonA * 100.0 / (firstServeWonA + firstServeWonB), 0)
            : 0;
        var winPct1B = (firstServeWonA + firstServeWonB) > 0
            ? Math.Round(firstServeWonB * 100.0 / (firstServeWonA + firstServeWonB), 0)
            : 0;
        var winPct2A = (secondServeWonA + secondServeWonB) > 0
            ? Math.Round(secondServeWonA * 100.0 / (secondServeWonA + secondServeWonB), 0)
            : 0;
        var winPct2B = (secondServeWonA + secondServeWonB) > 0
            ? Math.Round(secondServeWonB * 100.0 / (secondServeWonA + secondServeWonB), 0)
            : 0;

        var breakOppA = pointDtos.Count(p => !p.IsUserWinner && p.PointType != "Double Fault");
        var breakOppB = pointDtos.Count(p => p.IsUserWinner && p.PointType != "Double Fault");
        var breakWonA = matchDto.SetScores.Count(s => s.Player2Games > s.Player1Games);
        var breakWonB = matchDto.SetScores.Count(s => s.Player2Games < s.Player1Games);

        var tiebreakA = matchDto.SetScores.Count(s =>
            s.TiebreakScore.HasValue && s.Player1Games > s.Player2Games);
        var tiebreakB = matchDto.SetScores.Count(s =>
            s.TiebreakScore.HasValue && s.Player2Games > s.Player1Games);

        var receivingA = pointDtos.Count(p =>
            !p.IsUserWinner && !p.IsFirstServe && p.PointType != "Ace");
        var receivingB = pointDtos.Count(p =>
            p.IsUserWinner && !p.IsFirstServe && p.PointType != "Ace");

        var pointsWonA = pointDtos.Count(p => p.IsUserWinner);
        var pointsWonB = pointDtos.Count(p => !p.IsUserWinner);

        var gamesWonA = matchDto.SetScores.Sum(s => s.Player1Games);
        var gamesWonB = matchDto.SetScores.Sum(s => s.Player2Games);

        var maxGamesInRowA = 0;
        var maxGamesInRowB = 0;
        var currA = 0;
        var currB = 0;
        foreach (var set in matchDto.SetScores)
        {
            if (set.Player1Games > set.Player2Games)
            {
                currA++;
                maxGamesInRowA = Math.Max(maxGamesInRowA, currA);
                currB = 0;
            }
            else
            {
                currB++;
                maxGamesInRowB = Math.Max(maxGamesInRowB, currB);
                currA = 0;
            }
        }

        var maxPointsInRowA = 0;
        var maxPointsInRowB = 0;
        var runA = 0;
        var runB = 0;
        foreach (var p in pointDtos)
        {
            if (p.IsUserWinner)
            {
                runA++;
                maxPointsInRowA = Math.Max(maxPointsInRowA, runA);
                runB = 0;
            }
            else
            {
                runB++;
                maxPointsInRowB = Math.Max(maxPointsInRowB, runB);
                runA = 0;
            }
        }

        var servicePointsWonA = pointDtos.Count(p =>
            p.IsUserWinner && (p.PointType == "Winner" || p.PointType == "Ace"));
        var servicePointsWonB = pointDtos.Count(p =>
            !p.IsUserWinner && (p.PointType == "Winner" || p.PointType == "Ace"));

        var serviceGamesA = matchDto.SetScores.Sum(s => s.Player1Games);
        var serviceGamesB = matchDto.SetScores.Sum(s => s.Player2Games);

        var totalShotsA = pointDtos
            .Where(p => p.IsUserWinner)
            .Sum(p => p.NumberOfShots);
        var totalShotsB = pointDtos
            .Where(p => !p.IsUserWinner)
            .Sum(p => p.NumberOfShots);

        var avgShotsA = pointsWonA > 0
            ? Math.Round((double)totalShotsA / pointsWonA, 1)
            : 0;
        var avgShotsB = pointsWonB > 0
            ? Math.Round((double)totalShotsB / pointsWonB, 1)
            : 0;

        var maxShotsA = pointDtos
            .Where(p => p.IsUserWinner)
            .Select(p => p.NumberOfShots)
            .DefaultIfEmpty(0)
            .Max();
        var maxShotsB = pointDtos
            .Where(p => !p.IsUserWinner)
            .Select(p => p.NumberOfShots)
            .DefaultIfEmpty(0)
            .Max();

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

            FirstServePctA = firstServePctA,
            FirstServePctB = firstServePctB,
            WinPctOnFirstServeA = winPct1A,
            WinPctOnFirstServeB = winPct1B,
            WinPctOnSecondServeA = winPct2A,
            WinPctOnSecondServeB = winPct2B,

            BreakPointsWonA = breakWonA,
            BreakPointsOpportunitiesA = breakOppA,
            BreakPointsWonB = breakWonB,
            BreakPointsOpportunitiesB = breakOppB,

            TiebreaksWonA = tiebreakA,
            TiebreaksWonB = tiebreakB,

            ReceivingPointsWonA = receivingA,
            ReceivingPointsWonB = receivingB,

            PointsWonA = pointsWonA,
            PointsWonB = pointsWonB,
            GamesWonA = gamesWonA,
            GamesWonB = gamesWonB,

            MaxGamesInARowA = maxGamesInRowA,
            MaxGamesInARowB = maxGamesInRowB,
            MaxPointsInARowA = maxPointsInRowA,
            MaxPointsInARowB = maxPointsInRowB,

            ServicePointsWonA = servicePointsWonA,
            ServicePointsWonB = servicePointsWonB,
            ServiceGamesWonA = serviceGamesA,
            ServiceGamesWonB = serviceGamesB,

            TotalShotsPlayerA = totalShotsA,
            TotalShotsPlayerB = totalShotsB,
            AvgShotsPerPointA = avgShotsA,
            AvgShotsPerPointB = avgShotsB,
            MaxShotsInPointA = maxShotsA,
            MaxShotsInPointB = maxShotsB
        };
    }
}

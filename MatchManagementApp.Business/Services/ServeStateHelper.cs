public class ServeStateService : IServeStateService
{
    private readonly IScorekeepingService _scorekeeping;

    public ServeStateService(IScorekeepingService scorekeeping)
    {
        _scorekeeping = scorekeeping;
    }

    public PointDto GetServeState(MatchDto match, List<PointDto> points)
    {
        var state = _scorekeeping.CalculateScore(match, points);

        var initial = match.InitialServer;
        var other = initial == "User" ? "Opponent" : "User";
        string currentServer;
        if (state.InTiebreak)
        {
            var parts = state.CurrentGameScore
                             .Split('-', StringSplitOptions.TrimEntries)
                             .Select(int.Parse)
                             .ToArray();
            int p1 = parts[0];
            int p2 = parts[1];
            int total = p1 + p2;

            if (total == 0)
            {
                currentServer = initial;
            }
            else
            {
                int group = (total + 1) / 2;
                currentServer = (group % 2 == 1) ? other : initial;
            }
        }
        else
        {
            int gamesPlayed =
                state.SetScores
                     .Take(state.SetScores.Count - 1)
                     .Sum(s => s.Player1Games + s.Player2Games)
                + state.SetScores.Last().Player1Games
                + state.SetScores.Last().Player2Games;

            currentServer = (gamesPlayed % 2 == 0) ? initial : other;
        }

        bool isFirstServe = true;
        if (points.Count > 0)
        {
            var last = points.Last();
            if (last.PointType == "Fault" && last.IsFirstServe)
            {
                isFirstServe = false;
            }
        }

        return new PointDto
        {
            CurrentServer = currentServer,
            IsFirstServe = isFirstServe
        };
    }
}

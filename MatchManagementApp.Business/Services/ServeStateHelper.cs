public class ServeStateService : IServeStateService
{
    private readonly IScorekeepingService _scorekeepingService;

    public ServeStateService(IScorekeepingService scorekeepingService)
    {
        _scorekeepingService = scorekeepingService;
    }

    public ServeState GetServeState(MatchDto match, List<PointDto> points)
    {
        string server = match.InitialServer;
        bool isFirstServe = true;

        foreach (var point in points)
        {
            if (IsGameOverByThisPoint(points, point, match))
            {
                server = (server == "User") ? "Opponent" : "User";
                isFirstServe = true;
                continue;
            }

            if (point.PointType == "Fault")
            {
                if (isFirstServe) isFirstServe = false;
                else isFirstServe = true;
            }
            else if (point.PointType == "Double Fault" ||
                     point.PointType == "Ace" ||
                     point.PointType == "Winner" ||
                     point.PointType == "Unforced Error" ||
                     point.PointType == "Forced Error")
            {
                isFirstServe = true;
            }
        }

        return new ServeState
        {
            CurrentServer = server,
            IsFirstServe = isFirstServe
        };
    }

    private bool IsGameOverByThisPoint(List<PointDto> points, PointDto current, MatchDto match)
    {
        int idx = points.IndexOf(current);
        var before = points.Take(idx).ToList();
        var after = points.Take(idx + 1).ToList();

        var beforeScore = _scorekeepingService.CalculateScore(match, before);
        var afterScore = _scorekeepingService.CalculateScore(match, after);

        if (beforeScore.SetScores.Count == afterScore.SetScores.Count)
        {
            var b = beforeScore.SetScores.Last();
            var a = afterScore.SetScores.Last();
            return (a.Player1Games > b.Player1Games) || (a.Player2Games > b.Player2Games);
        }
        else
        {
            return true;
        }
    }
}

public class ServeStateService : IServeStateService
{
    private readonly IScorekeepingService _scorekeepingService;

    public ServeStateService(IScorekeepingService scorekeepingService)
    {
        _scorekeepingService = scorekeepingService;
    }

    public PointDto GetServeState(MatchDto match, List<PointDto> points)
    {
        var score = _scorekeepingService.CalculateScore(match, points);
        int gamesPlayed = score.SetScores.Sum(s => s.Player1Games + s.Player2Games);

        string initialServer = match.InitialServer; // "User" or "Opponent"
        string currentServer = (gamesPlayed % 2 == 0) ? initialServer : (initialServer == "User" ? "Opponent" : "User");

        // Determine serve number (first/second) from the most recent point
        bool isFirstServe = true;
        if (points.Count > 0)
        {
            var last = points.Last();
            if (last.PointType == "Fault" && last.IsFirstServe)
                isFirstServe = false;
        }

        // Return a PointDto with only these two properties set
        return new PointDto
        {
            CurrentServer = currentServer,
            IsFirstServe = isFirstServe
        };
    }
}

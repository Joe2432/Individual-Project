public class PointEntity
{
    public int Id { get; private set; }
    public string WinnerLabel { get; private set; } = string.Empty;
    public string WinningMethod { get; private set; } = string.Empty;
    public int NrShots { get; private set; }
    public int MatchId { get; private set; }
    public bool IsFirstServe { get; private set; } 

    public MatchEntity Match { get; private set; } = null!;

    public PointEntity(int matchId, string winnerLabel, string? winningMethod, int nrShots, bool isFirstServe)
    {
        MatchId = matchId;
        WinnerLabel = winnerLabel;
        WinningMethod = winningMethod ?? "";
        NrShots = nrShots;
        IsFirstServe = isFirstServe;
    }

}

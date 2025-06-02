public class PointEntity
{
    public int Id { get; set; }
    public string WinnerLabel { get; set; } = string.Empty;
    public string WinningMethod { get; set; } = string.Empty;
    public int NrShots { get; set; }
    public int MatchId { get; set; }
    public bool IsFirstServe { get; set; } 

    public MatchEntity Match { get; set; } = null!;

    public PointEntity(int matchId, string winnerLabel, string? winningMethod, int nrShots, bool isFirstServe)
    {
        MatchId = matchId;
        WinnerLabel = winnerLabel;
        WinningMethod = winningMethod ?? "";
        NrShots = nrShots;
        IsFirstServe = isFirstServe;
    }

}

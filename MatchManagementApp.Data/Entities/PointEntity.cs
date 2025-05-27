public class PointEntity
{
    public int Id { get; set; }
    public string WinnerLabel { get; set; } = null!; // "User" or "Opponent"
    public string WinningMethod { get; set; } = null!;
    public int NrShots { get; set; }

    public int MatchId { get; set; }
    public MatchEntity Match { get; set; } = null!;

    public PointEntity() { }

    public PointEntity(int matchId, string winnerLabel, string winningMethod, int nrShots)
    {
        MatchId = matchId;
        WinnerLabel = winnerLabel;
        WinningMethod = winningMethod;
        NrShots = nrShots;
    }
}

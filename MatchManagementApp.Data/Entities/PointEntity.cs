public class PointEntity
{
    public int Id { get; set; }
    public string WinningMethod { get; set; } = null!;
    public int NrShots { get; set; }

    public int MatchId { get; set; }
    public MatchEntity Match { get; set; } = null!;

    public int WinnerId { get; set; }
    public UserEntity Winner { get; set; } = null!;

    public PointEntity() { }

    public PointEntity(int matchId, int winnerId, string winningMethod, int nrShots)
    {
        MatchId = matchId;
        WinnerId = winnerId;
        WinningMethod = winningMethod;
        NrShots = nrShots;
    }
}

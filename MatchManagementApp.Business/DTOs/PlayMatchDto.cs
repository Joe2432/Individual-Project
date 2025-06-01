public class PlayMatchDto
{
    public int MatchId { get; set; }
    public string MatchType { get; set; } = string.Empty;
    public string? PartnerName { get; set; }
    public string Opponent1 { get; set; } = string.Empty;
    public string? Opponent2 { get; set; }
    public string Surface { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    // ─── Replace the old raw‐string Score with a structured ScoreDto ───
    public ScoreDto Score { get; set; } = new ScoreDto();

    // You still need the three lists for per‐set game totals:
    public List<int> DisplaySetIndices { get; set; } = new List<int>();
    public List<int> UserSetGames { get; set; } = new List<int>();
    public List<int> OpponentSetGames { get; set; } = new List<int>();
}
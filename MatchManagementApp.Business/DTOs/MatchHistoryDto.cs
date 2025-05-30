public class MatchHistoryDto
{
    public MatchDto Match { get; set; } = null!;
    public string ScoreSummary { get; set; } = string.Empty;
    public bool MatchOver { get; set; }

    public DateTime? MatchDate { get; set; } 
}

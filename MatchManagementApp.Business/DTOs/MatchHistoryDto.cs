public class MatchHistoryDto
{
    public MatchDto Match { get; set; }
    public string ScoreSummary { get; set; } = string.Empty;
    public bool MatchOver { get; set; }
}

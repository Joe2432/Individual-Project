public class MatchHistoryViewModel
{
	public int MatchId { get; set; }
	public string MatchType { get; set; } = string.Empty;
	public string PartnerName { get; set; } = "-";
	public string FirstOpponentName { get; set; } = string.Empty;
	public string SecondOpponentName { get; set; } = "-";
	public string Surface { get; set; } = string.Empty;
	public string ScoreSummary { get; set; } = string.Empty;
	public string Status { get; set; } = "In Progress";
	public bool MatchOver { get; set; }
    public DateTime? MatchDate { get; set; }
}

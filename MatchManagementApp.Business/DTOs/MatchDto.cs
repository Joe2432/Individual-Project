public class MatchDto
{
    public int Id { get; set; }
    public int CreatedByUserId { get; set; }
    public string MatchType { get; set; } = string.Empty;
    public string? PartnerName { get; set; }
    public string FirstOpponentName { get; set; } = string.Empty;
    public string? SecondOpponentName { get; set; }
    public string Surface { get; set; } = string.Empty;
    public DateTime? MatchDate { get; set; }
    public int NrSets { get; set; }
    public string FinalSetType { get; set; } = string.Empty;
    public string GameFormat { get; set; } = string.Empty;

    public List<SetScoreDto> SetScores { get; set; } = new();
    public string CurrentGameScore { get; set; } = string.Empty;
    public bool InTiebreak { get; set; }
    public bool MatchOver { get; set; }

    public List<int> DisplaySetIndices { get; set; } = new();
    public List<string> UserSetGames { get; set; } = new();
    public List<string> OpponentSetGames { get; set; } = new();
    public string GameUserDisplay { get; set; } = "";
    public string GameOpponentDisplay { get; set; } = "";
    public string? Score { get; set; }
    public string? ScoreSummary { get; set; }
}

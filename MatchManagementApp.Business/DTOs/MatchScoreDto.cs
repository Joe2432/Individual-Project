public class MatchScoreDto
{
    public int UserSetsWon { get; set; }
    public int OpponentSetsWon { get; set; }
    public List<SetScoreDto> SetScores { get; set; } = new();
    public string CurrentGameScore { get; set; } = string.Empty;
    public bool InTiebreak { get; set; }
    public bool MatchOver { get; set; }
}

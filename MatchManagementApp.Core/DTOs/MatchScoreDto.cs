public class MatchScoreDto
{
    public int Player1SetsWon { get; set; }
    public int Player2SetsWon { get; set; }
    public List<SetScoreDto> SetScores { get; set; } = new();
    public string CurrentGameScore { get; set; } = string.Empty;
    public bool InTiebreak { get; set; }
    public bool MatchOver { get; set; }
}

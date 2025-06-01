public class ScoreDto
{
    public bool InTiebreak { get; set; }
    public string CurrentGameScore { get; set; } = string.Empty;
    public string GameUserDisplay { get; set; } = string.Empty;
    public string GameOpponentDisplay { get; set; } = string.Empty;
}

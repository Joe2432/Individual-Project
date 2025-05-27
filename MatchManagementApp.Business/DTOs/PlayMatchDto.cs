public class PlayMatchDto
{
    public MatchScoreDto Score { get; set; }
    public List<int> DisplaySetIndices { get; set; } = new();
    public List<string> UserSetGames { get; set; } = new();
    public List<string> OpponentSetGames { get; set; } = new();
    public string GameUserDisplay { get; set; } = "";
    public string GameOpponentDisplay { get; set; } = "";
}

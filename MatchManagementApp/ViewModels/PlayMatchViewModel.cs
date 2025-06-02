public class PlayMatchViewModel
{
    public List<int> DisplaySetIndices { get; set; } = new();
    public List<int> UserSetGames { get; set; } = new();
    public List<int> OpponentSetGames { get; set; } = new();
    public string? GameUserDisplay { get; set; }
    public string? GameOpponentDisplay { get; set; }
    public bool MatchOver { get; set; }
    public ScoreViewModel Score { get; set; } = new();
    public string CurrentServer { get; set; } = "User";
    public bool IsFirstServe { get; set; } = true;
}

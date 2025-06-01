public class PlayMatchViewModel
{
    public MatchScoreDto Score { get; set; }
    public List<int> DisplaySetIndices { get; set; }
    public List<string> UserSetGames { get; set; }
    public List<string> OpponentSetGames { get; set; }
    public string GameUserDisplay { get; set; }
    public string GameOpponentDisplay { get; set; }
}

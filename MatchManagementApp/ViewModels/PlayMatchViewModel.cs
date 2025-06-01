// PlayMatchViewModel.cs
using System;
using System.Collections.Generic;

public class PlayMatchViewModel
{
    public int MatchId { get; set; }
    public string MatchType { get; set; } = string.Empty;
    public string? PartnerName { get; set; }
    public string Opponent1 { get; set; } = string.Empty;
    public string? Opponent2 { get; set; }
    public string Surface { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<int> DisplaySetIndices { get; set; } = new();
    public List<int> UserSetGames { get; set; } = new();
    public List<int> OpponentSetGames { get; set; } = new();
    public bool InTiebreak { get; set; }
    public string CurrentGameScore { get; set; } = string.Empty;
    public string GameUserDisplay { get; set; } = string.Empty;
    public string GameOpponentDisplay { get; set; } = string.Empty;
}

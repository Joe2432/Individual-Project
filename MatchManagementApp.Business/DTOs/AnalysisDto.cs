public class AnalysisDto
{
    public int MatchId { get; set; }
    public string PlayerAName { get; set; } = string.Empty;
    public string PlayerBName { get; set; } = string.Empty;
    public DateTime? MatchDate { get; set; }
    public List<string> PlayerAGameByGame { get; set; } = new();
    public List<string> PlayerBGameByGame { get; set; } = new();
    public int AcesPlayerA { get; set; }
    public int AcesPlayerB { get; set; }
    public int DoubleFaultsPlayerA { get; set; }
    public int DoubleFaultsPlayerB { get; set; }
    public int WinnersPlayerA { get; set; }
    public int WinnersPlayerB { get; set; }
    public int UnforcedErrorsPlayerA { get; set; }
    public int UnforcedErrorsPlayerB { get; set; }
    public int ForcedErrorsPlayerA { get; set; }
    public int ForcedErrorsPlayerB { get; set; }
    public int TotalPointsPlayerA { get; set; }
    public int TotalPointsPlayerB { get; set; }
    public int TotalGamesPlayerA { get; set; }
    public int TotalGamesPlayerB { get; set; }
}
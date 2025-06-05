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

    public double FirstServePctA { get; set; }
    public double FirstServePctB { get; set; }
    public double WinPctOnFirstServeA { get; set; }
    public double WinPctOnFirstServeB { get; set; }
    public double WinPctOnSecondServeA { get; set; }
    public double WinPctOnSecondServeB { get; set; }

    public int BreakPointsWonA { get; set; }
    public int BreakPointsOpportunitiesA { get; set; }
    public int BreakPointsWonB { get; set; }
    public int BreakPointsOpportunitiesB { get; set; }

    public int TiebreaksWonA { get; set; }
    public int TiebreaksWonB { get; set; }

    public int ReceivingPointsWonA { get; set; }
    public int ReceivingPointsWonB { get; set; }

    public int PointsWonA { get; set; }
    public int PointsWonB { get; set; }
    public int GamesWonA { get; set; }
    public int GamesWonB { get; set; }

    public int MaxGamesInARowA { get; set; }
    public int MaxGamesInARowB { get; set; }
    public int MaxPointsInARowA { get; set; }
    public int MaxPointsInARowB { get; set; }

    public int ServicePointsWonA { get; set; }
    public int ServicePointsWonB { get; set; }
    public int ServiceGamesWonA { get; set; }
    public int ServiceGamesWonB { get; set; }

    // Shot-related metrics
    public int TotalShotsPlayerA { get; set; }
    public int TotalShotsPlayerB { get; set; }
    public double AvgShotsPerPointA { get; set; }
    public double AvgShotsPerPointB { get; set; }
    public int MaxShotsInPointA { get; set; }
    public int MaxShotsInPointB { get; set; }
}

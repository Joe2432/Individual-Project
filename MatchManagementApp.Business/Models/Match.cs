public class Match
{
    public int Id { get; private set; }
    public int CreatedByUserId { get; private set; }
    public string MatchType { get; private set; }
    public string FirstOpponentName { get; private set; }
    public string? PartnerName { get; private set; }
    public string? SecondOpponentName { get; private set; }
    public string Surface { get; private set; }
    public int NrSets { get; private set; }
    public string FinalSetType { get; private set; }
    public string GameFormat { get; private set; }
    public DateTime? MatchDate { get; private set; }

    public List<Point> Points { get; private set; } = new();

    public Match(int Id, int createdByUserId, string matchType, string firstOpponentName, string surface,
                 int nrSets, string finalSetType, string gameFormat, string? partnerName,
                 string? secondOpponentName, DateTime? matchDate)
    {
        Id = Id;
        CreatedByUserId = createdByUserId;
        MatchType = matchType;
        FirstOpponentName = firstOpponentName;
        Surface = surface;
        NrSets = nrSets;
        FinalSetType = finalSetType;
        GameFormat = gameFormat;
        PartnerName = partnerName;
        SecondOpponentName = secondOpponentName;
        MatchDate = matchDate;
    }

    public void SetId(int id)
    {
        Id = id;
    }

    public void AddPoint(Point point)
    {
        if (point.MatchId != Id)
            throw new InvalidOperationException("Point does not belong to this match.");
        Points.Add(point);
    }

    public void RemoveLastPoint()
    {
        if (Points.Any())
            Points.RemoveAt(Points.Count - 1);
    }

    public List<Point> GetPoints() => Points.ToList();

    public MatchScore CalculateScore()
    {
        return MatchScoringEngine.Calculate(this);
    }

    public bool IsOver()
    {
        var score = CalculateScore();
        return score.MatchOver;
    }

    public string GetScoreDisplay()
    {
        var score = CalculateScore();
        var summary = string.Join(" ", score.SetScores);
        return summary + " | " + score.CurrentGameScore;
    }
}
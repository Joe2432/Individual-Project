using System.Text;

public class MatchScore
{
    public int UserSetsWon { get; set; }
    public int OpponentSetsWon { get; set; }
    public List<SetScore> SetScores { get; set; } = new();
    public string CurrentGameScore { get; set; } = string.Empty;
    public bool InTiebreak { get; set; }
    public bool MatchOver { get; set; }

    public string DisplaySetScore()
    {
        return $"Sets: {UserSetsWon}-{OpponentSetsWon}";
    }

    public string DisplayCurrentGameScore()
    {
        return $"Current Game: {CurrentGameScore}";
    }

    public string Display()
    {
        var builder = new StringBuilder();
        builder.Append(DisplaySetScore());
        builder.Append(" | ");
        builder.Append(DisplayCurrentGameScore());
        return builder.ToString();
    }
    public string GetScoreSummary()
    {
        return string.Join(" ", SetScores.Select(s => s.ToString()));
    }
}
public class ScoreViewModel
{
    public int UserPoints { get; set; }
    public int OpponentPoints { get; set; }
    public string Display => $"{UserPoints} - {OpponentPoints}";
}

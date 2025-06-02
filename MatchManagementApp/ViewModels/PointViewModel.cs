public class PointViewModel
{
    public int MatchId { get; set; }
    public string PointType { get; set; } = string.Empty; 
    public int NumberOfShots { get; set; } = 1;
    public bool IsUserWinner { get; set; }
    public bool IsFirstServe { get; set; }
}

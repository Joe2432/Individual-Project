public class PointCreateDto
{
    public string PointType { get; set; } = null!;
    public int NumberOfShots { get; set; }
    public bool IsUserWinner { get; set; }
    public int MatchId { get; set; }
}

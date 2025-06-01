public class Point
{
    public int MatchId { get; private set; }
    public string PointType { get; private set; } // e.g., Rally, Ace, Double Fault
    public int NumberOfShots { get; private set; }
    public bool IsUserWinner { get; private set; }

    public Point(int matchId, string pointType, int numberOfShots, bool isUserWinner)
    {
        MatchId = matchId;
        PointType = pointType;
        NumberOfShots = numberOfShots;
        IsUserWinner = isUserWinner;
    }
}
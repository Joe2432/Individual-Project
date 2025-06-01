public class SetScore
{
    public int UserGames { get; set; }
    public int OpponentGames { get; set; }
    public int? TiebreakScore { get; set; }

    public SetScore(int userGames = 0, int opponentGames = 0, int? tiebreakScore = null)
    {
        UserGames = userGames;
        OpponentGames = opponentGames;
        TiebreakScore = tiebreakScore;
    }

    public bool IsEmpty => UserGames == 0 && OpponentGames == 0;

    public override string ToString()
    {
        if (TiebreakScore.HasValue)
        {
            return $"{UserGames}-{OpponentGames}({TiebreakScore})";
        }
        return $"{UserGames}-{OpponentGames}";
    }
}
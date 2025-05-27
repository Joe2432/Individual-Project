public class SetScoreDto
{
    public int Player1Games { get; set; }
    public int Player2Games { get; set; }
    public int? TiebreakScore { get; set; } // null = no tiebreak

    public override string ToString()
    {
        if (TiebreakScore.HasValue)
        {
            return $"{Player1Games}-{Player2Games}({TiebreakScore})";
        }
        return $"{Player1Games}-{Player2Games}";
    }
}

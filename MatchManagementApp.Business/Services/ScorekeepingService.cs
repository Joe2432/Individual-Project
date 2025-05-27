public class ScorekeepingService : IScorekeepingService
{
    public MatchScoreDto CalculateScore(MatchEntity match, IEnumerable<PointEntity> pointList)
    {
        var points = pointList.ToList(); // Ensure single enumeration

        var result = new MatchScoreDto();
        var sets = new List<SetScoreDto> { new() };
        bool noAd = match.GameFormat.Equals("NoAd", StringComparison.OrdinalIgnoreCase);
        bool tiebreakFinalSet = match.FinalSetType.Equals("Tiebreak", StringComparison.OrdinalIgnoreCase);
        int maxSetsToWin = (match.NrSets + 1) / 2;

        int p1Points = 0, p2Points = 0;
        int p1Games = 0, p2Games = 0;
        int p1Sets = 0, p2Sets = 0;
        int p1Tiebreak = 0, p2Tiebreak = 0;
        int setIndex = 0;
        bool inTiebreak = false;
        bool matchOver = false;

        foreach (var point in points)
        {
            var winner = point.WinnerLabel == "User" ? 1 : 2;

            if (inTiebreak)
            {
                if (winner == 1) p1Tiebreak++;
                else p2Tiebreak++;

                if ((p1Tiebreak >= 7 || p2Tiebreak >= 7) && Math.Abs(p1Tiebreak - p2Tiebreak) >= 2)
                {
                    if (p1Tiebreak > p2Tiebreak) p1Games++;
                    else p2Games++;

                    sets[setIndex].Player1Games = p1Games;
                    sets[setIndex].Player2Games = p2Games;
                    sets[setIndex].TiebreakScore = Math.Max(p1Tiebreak, p2Tiebreak);

                    p1Games = p2Games = 0;
                    p1Tiebreak = p2Tiebreak = 0;
                    setIndex++;
                    sets.Add(new SetScoreDto());
                    inTiebreak = false;
                }
            }
            else
            {
                if (winner == 1) p1Points++;
                else p2Points++;

                if (noAd)
                {
                    if (p1Points >= 4 || p2Points >= 4)
                    {
                        if (p1Points > p2Points)
                        {
                            p1Games++; p1Points = p2Points = 0;
                        }
                        else
                        {
                            p2Games++; p1Points = p2Points = 0;
                        }
                    }
                }
                else
                {
                    if ((p1Points >= 4 || p2Points >= 4) && Math.Abs(p1Points - p2Points) >= 2)
                    {
                        if (p1Points > p2Points)
                        {
                            p1Games++; p1Points = p2Points = 0;
                        }
                        else
                        {
                            p2Games++; p1Points = p2Points = 0;
                        }
                    }
                }

                if ((p1Games == 6 && p2Games == 6))
                {
                    bool isFinalSet = (p1Sets + p2Sets + 1) == match.NrSets;

                    if (!isFinalSet || (isFinalSet && tiebreakFinalSet))
                    {
                        inTiebreak = true;
                    }
                    // else: final set continues normally
                }
                else if ((p1Games >= 6 || p2Games >= 6) && Math.Abs(p1Games - p2Games) >= 2)
                {
                    if (p1Games > p2Games) p1Sets++;
                    else p2Sets++;

                    sets[setIndex].Player1Games = p1Games;
                    sets[setIndex].Player2Games = p2Games;

                    p1Games = p2Games = 0;
                    setIndex++;
                    sets.Add(new SetScoreDto());
                }
            }

            if (p1Sets == maxSetsToWin || p2Sets == maxSetsToWin)
            {
                result.MatchOver = true;
                matchOver = true;
                break;
            }
        }

        if (!matchOver)
        {
            sets[setIndex].Player1Games = p1Games;
            sets[setIndex].Player2Games = p2Games;
        }

        result.Player1SetsWon = p1Sets;
        result.Player2SetsWon = p2Sets;
        result.SetScores = sets;
        result.InTiebreak = inTiebreak;
        result.CurrentGameScore = FormatGameScore(p1Points, p2Points, inTiebreak, noAd);

        return result;
    }

    private string FormatGameScore(int p1, int p2, bool tiebreak, bool noAd)
    {
        if (tiebreak) return $"{p1} - {p2}";

        string[] scores = { "0", "15", "30", "40", "Ad" };

        if (p1 >= 3 && p2 >= 3)
        {
            if (p1 == p2) return "Deuce";
            if (p1 > p2) return "Ad - 40";
            return "40 - Ad";
        }

        string s1 = p1 >= scores.Length ? "Game" : scores[p1];
        string s2 = p2 >= scores.Length ? "Game" : scores[p2];

        return $"{s1} - {s2}";
    }
}

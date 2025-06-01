public static class MatchScoringEngine
{
    private const int GAMES_TO_WIN_SET = 6;
    private const int SET_TIEBREAK_MIN_GAMES = 6;
    private const int TIEBREAK_POINTS = 7;
    private const int SUPER_TIEBREAK_POINTS = 10;

    public static MatchScore Calculate(Match match)
    {
        var result = new MatchScore();
        var sets = new List<SetScore> { new() };

        var points = match.Points;
        int maxSets = match.NrSets;
        int setsToWin = (maxSets + 1) / 2;
        bool noAd = match.GameFormat.Equals("NoAd", StringComparison.OrdinalIgnoreCase);
        bool decisivePoint = match.GameFormat.Contains("Decisive", StringComparison.OrdinalIgnoreCase);
        bool maxiTiebreakFinalSet = match.FinalSetType.Equals("Maxi Tiebreak", StringComparison.OrdinalIgnoreCase);

        int p1Points = 0, p2Points = 0;
        int p1Games = 0, p2Games = 0;
        int p1Sets = 0, p2Sets = 0;
        int p1Tiebreak = 0, p2Tiebreak = 0;
        int setIndex = 0;
        bool inTiebreak = false;
        bool inMaxiTiebreak = false;
        bool matchOver = false;

        foreach (var point in points)
        {
            int winner = point.IsUserWinner ? 1 : 2;

            bool isFinalSet = (p1Sets + p2Sets + 1) == maxSets;
            if (isFinalSet && maxiTiebreakFinalSet)
            {
                inMaxiTiebreak = true;
            }
            else if (p1Games == SET_TIEBREAK_MIN_GAMES && p2Games == SET_TIEBREAK_MIN_GAMES)
            {
                inTiebreak = true;
            }

            if (inMaxiTiebreak)
            {
                if (winner == 1) p1Tiebreak++; else p2Tiebreak++;

                if ((p1Tiebreak >= SUPER_TIEBREAK_POINTS || p2Tiebreak >= SUPER_TIEBREAK_POINTS)
                    && Math.Abs(p1Tiebreak - p2Tiebreak) >= 2)
                {
                    if (p1Tiebreak > p2Tiebreak) p1Sets++; else p2Sets++;
                    sets[setIndex] = new SetScore(p1Tiebreak, p2Tiebreak);
                    matchOver = true;
                    break;
                }
            }
            else if (inTiebreak)
            {
                if (winner == 1) p1Tiebreak++; else p2Tiebreak++;

                if ((p1Tiebreak >= TIEBREAK_POINTS || p2Tiebreak >= TIEBREAK_POINTS)
                    && Math.Abs(p1Tiebreak - p2Tiebreak) >= 2)
                {
                    if (p1Tiebreak > p2Tiebreak) p1Games++; else p2Games++;
                    sets[setIndex] = new SetScore(p1Games, p2Games, Math.Min(p1Tiebreak, p2Tiebreak));

                    if (p1Games > p2Games) p1Sets++; else p2Sets++;

                    p1Games = p2Games = p1Tiebreak = p2Tiebreak = 0;
                    setIndex++;
                    if (p1Sets < setsToWin && p2Sets < setsToWin && (p1Sets + p2Sets) < maxSets)
                        sets.Add(new SetScore());

                    inTiebreak = false;
                }
            }
            else
            {
                if (winner == 1) p1Points++; else p2Points++;

                if (noAd || decisivePoint)
                {
                    if ((p1Points >= 4 || p2Points >= 4))
                    {
                        if (p1Points > p2Points) p1Games++; else p2Games++;
                        p1Points = p2Points = 0;
                    }
                }
                else
                {
                    if ((p1Points >= 4 || p2Points >= 4) && Math.Abs(p1Points - p2Points) >= 2)
                    {
                        if (p1Points > p2Points) p1Games++; else p2Games++;
                        p1Points = p2Points = 0;
                    }
                }

                if ((p1Games >= GAMES_TO_WIN_SET || p2Games >= GAMES_TO_WIN_SET)
                    && Math.Abs(p1Games - p2Games) >= 2)
                {
                    if (p1Games > p2Games) p1Sets++; else p2Sets++;
                    sets[setIndex] = new SetScore(p1Games, p2Games);
                    p1Games = p2Games = 0;
                    setIndex++;
                    if (p1Sets < setsToWin && p2Sets < setsToWin && (p1Sets + p2Sets) < maxSets)
                        sets.Add(new SetScore());
                }
            }

            if (p1Sets == setsToWin || p2Sets == setsToWin)
            {
                result.MatchOver = true;
                matchOver = true;
                break;
            }
        }

        if (!matchOver && setIndex < sets.Count)
        {
            sets[setIndex].UserGames = p1Games;
            sets[setIndex].OpponentGames = p2Games;
        }

        result.UserSetsWon = p1Sets;
        result.OpponentSetsWon = p2Sets;
        result.SetScores = sets;
        result.InTiebreak = inTiebreak || inMaxiTiebreak;
        result.MatchOver = matchOver;
        result.CurrentGameScore = inTiebreak || inMaxiTiebreak
            ? $"{p1Tiebreak} - {p2Tiebreak}"
            : FormatGameScore(p1Points, p2Points, false, noAd || decisivePoint);

        return result;
    }

    private static string FormatGameScore(int p1, int p2, bool tiebreak, bool suddenDeath)
    {
        if (tiebreak) return $"{p1} - {p2}";
        string[] scores = { "0", "15", "30", "40", "Ad" };

        if (p1 >= 3 && p2 >= 3)
        {
            if (p1 == p2) return suddenDeath ? "40-40" : "Deuce";
            if (p1 > p2) return "Ad - 40";
            return "40 - Ad";
        }

        string s1 = p1 >= scores.Length ? "Game" : scores[p1];
        string s2 = p2 >= scores.Length ? "Game" : scores[p2];
        return $"{s1} - {s2}";
    }
}

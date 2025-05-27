public class ScorekeepingService : IScorekeepingService
{
    // --- Tennis scoring rules ---
    private const int GAMES_TO_WIN_SET = 6;               // 6 games to win a set (must win by 2, unless tiebreak)
    private const int SET_TIEBREAK_MIN_GAMES = 6;         // Tiebreak at 6-6 in games
    private const int TIEBREAK_POINTS = 7;                // Standard tiebreak: first to 7, win by 2
    private const int SUPER_TIEBREAK_POINTS = 10;         // Super tiebreak (Maxi Tiebreak): first to 10, win by 2

    public MatchScoreDto CalculateScore(MatchReadDto match, IEnumerable<PointReadDto> pointList)
    {
        var points = pointList.ToList();
        var result = new MatchScoreDto();
        var sets = new List<SetScoreDto> { new() };

        int maxSets = match.NrSets; // From DB (3 or 5)
        int setsToWin = (maxSets + 1) / 2; // e.g. 2 for 3, 3 for 5

        bool noAd = match.GameFormat.Equals("NoAd", StringComparison.OrdinalIgnoreCase);
        bool decisivePoint = match.GameFormat.Contains("Decisive", StringComparison.OrdinalIgnoreCase); // For 40-40 sudden death
        bool maxiTiebreakFinalSet = match.FinalSetType.Equals("Maxi Tiebreak", StringComparison.OrdinalIgnoreCase);

        int p1Points = 0, p2Points = 0;
        int p1Games = 0, p2Games = 0;
        int p1Sets = 0, p2Sets = 0;
        int p1Tiebreak = 0, p2Tiebreak = 0;
        int setIndex = 0;
        bool inTiebreak = false, inMaxiTiebreak = false;
        bool matchOver = false;

        foreach (var point in points)
        {
            var winner = point.IsUserWinner ? 1 : 2;

            // --- Maxi/Super Tiebreak logic for the last set ---
            if (inMaxiTiebreak)
            {
                if (winner == 1) p1Tiebreak++;
                else p2Tiebreak++;

                if ((p1Tiebreak >= SUPER_TIEBREAK_POINTS || p2Tiebreak >= SUPER_TIEBREAK_POINTS) && Math.Abs(p1Tiebreak - p2Tiebreak) >= 2)
                {
                    // Winner of the super tiebreak wins the final set
                    if (p1Tiebreak > p2Tiebreak) p1Sets++;
                    else p2Sets++;

                    sets[setIndex].Player1Games = 0;
                    sets[setIndex].Player2Games = 0;
                    sets[setIndex].TiebreakScore = Math.Max(p1Tiebreak, p2Tiebreak);

                    matchOver = true;
                    break;
                }
            }
            // --- Standard tiebreak logic ---
            else if (inTiebreak)
            {
                if (winner == 1) p1Tiebreak++;
                else p2Tiebreak++;

                if ((p1Tiebreak >= TIEBREAK_POINTS || p2Tiebreak >= TIEBREAK_POINTS) && Math.Abs(p1Tiebreak - p2Tiebreak) >= 2)
                {
                    if (p1Tiebreak > p2Tiebreak) p1Games++;
                    else p2Games++;

                    sets[setIndex].Player1Games = p1Games;
                    sets[setIndex].Player2Games = p2Games;
                    sets[setIndex].TiebreakScore = Math.Max(p1Tiebreak, p2Tiebreak);

                    // Set winner is player who won tiebreak
                    if (p1Games > p2Games) p1Sets++;
                    else p2Sets++;

                    p1Games = p2Games = 0;
                    p1Tiebreak = p2Tiebreak = 0;
                    setIndex++;
                    if (p1Sets < setsToWin && p2Sets < setsToWin && (p1Sets + p2Sets) < maxSets)
                        sets.Add(new SetScoreDto());
                    inTiebreak = false;
                }
            }
            else
            {
                // --- Normal game logic ---
                if (winner == 1) p1Points++;
                else p2Points++;

                // NoAd/Decisive Point: game won at 4th point, regardless of margin (sudden death at deuce)
                if (noAd || decisivePoint)
                {
                    if ((p1Points >= 4 || p2Points >= 4) && Math.Abs(p1Points - p2Points) >= (decisivePoint ? 1 : 0))
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
                // With Advantage: must win by 2 after 40-40
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

                // Set logic: Tiebreak or Maxi Tiebreak for the final set
                bool isFinalSet = (p1Sets + p2Sets + 1) == maxSets;

                // --- Start Maxi Tiebreak for final set if needed ---
                if (isFinalSet && maxiTiebreakFinalSet)
                {
                    if ((p1Games == 0 && p2Games == 0) && (p1Points == 0 && p2Points == 0) && !inMaxiTiebreak)
                    {
                        inMaxiTiebreak = true;
                        continue;
                    }
                }
                // --- Start Standard Tiebreak at 6-6 if not in final set or not maxi tiebreak ---
                else if (p1Games == SET_TIEBREAK_MIN_GAMES && p2Games == SET_TIEBREAK_MIN_GAMES)
                {
                    if (!isFinalSet || (isFinalSet && !maxiTiebreakFinalSet))
                    {
                        inTiebreak = true;
                    }
                }
                // --- Win set by 2 games margin, except at 6-6 (handled above) ---
                else if ((p1Games >= GAMES_TO_WIN_SET || p2Games >= GAMES_TO_WIN_SET) && Math.Abs(p1Games - p2Games) >= 2)
                {
                    if (p1Games > p2Games) p1Sets++;
                    else p2Sets++;

                    sets[setIndex].Player1Games = p1Games;
                    sets[setIndex].Player2Games = p2Games;

                    p1Games = p2Games = 0;
                    setIndex++;
                    if (p1Sets < setsToWin && p2Sets < setsToWin && (p1Sets + p2Sets) < maxSets)
                        sets.Add(new SetScoreDto());
                }
            }

            // --- Match over if required sets are won, no more sets to be played ---
            if (p1Sets == setsToWin || p2Sets == setsToWin)
            {
                result.MatchOver = true;
                matchOver = true;
                break;
            }
        }

        // Final set games for display (if match not over)
        if (!matchOver && setIndex < sets.Count)
        {
            sets[setIndex].Player1Games = p1Games;
            sets[setIndex].Player2Games = p2Games;
        }

        result.Player1SetsWon = p1Sets;
        result.Player2SetsWon = p2Sets;
        result.SetScores = sets;
        result.InTiebreak = inTiebreak || inMaxiTiebreak;
        result.CurrentGameScore = (inMaxiTiebreak)
            ? $"{p1Tiebreak} - {p2Tiebreak}"
            : FormatGameScore(p1Points, p2Points, inTiebreak, noAd || decisivePoint);

        return result;
    }

    private string FormatGameScore(int p1, int p2, bool tiebreak, bool suddenDeath)
    {
        if (tiebreak) return $"{p1} - {p2}";

        string[] scores = { "0", "15", "30", "40", "Ad" };

        if (p1 >= 3 && p2 >= 3)
        {
            if (p1 == p2) return suddenDeath ? "40-40 (SD)" : "Deuce";
            if (p1 > p2) return "Ad - 40";
            return "40 - Ad";
        }

        string s1 = p1 >= scores.Length ? "Game" : scores[p1];
        string s2 = p2 >= scores.Length ? "Game" : scores[p2];

        return $"{s1} - {s2}";
    }
}

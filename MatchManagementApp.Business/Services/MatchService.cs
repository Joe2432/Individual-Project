using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPointRepository _pointRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScorekeepingService _scorekeepingService;

    public MatchService(
        IMatchRepository matchRepository,
        IPointRepository pointRepository,
        IUserRepository userRepository,
        IScorekeepingService scorekeepingService) // <-- Make sure to inject this
    {
        _matchRepository = matchRepository;
        _pointRepository = pointRepository;
        _userRepository = userRepository;
        _scorekeepingService = scorekeepingService;
    }

    public async Task<int> CreateMatchAsync(MatchCreateDto dto)
    {
        Console.WriteLine($"[MatchService] Received match creation request for user ID: {dto.CreatedByUserId}");

        var user = await _userRepository.GetUserByIdAsync(dto.CreatedByUserId);

        if (user == null)
        {
            Console.WriteLine($"[MatchService] ERROR: No user found in DB with ID {dto.CreatedByUserId}");
            throw new InvalidOperationException("Cannot create match: user does not exist.");
        }

        Console.WriteLine($"[MatchService] User verified. Proceeding to create match.");
        return await _matchRepository.CreateMatchAsync(dto);
    }

    public async Task<List<MatchReadDto>> GetUserMatchesAsync(int userId)
    {
        Console.WriteLine($"[MatchService] Fetching matches for user ID: {userId}");
        return await _matchRepository.GetMatchesByUserIdAsync(userId);
    }

    public async Task<string> GetScoreDisplayAsync(int matchId, int userId)
    {
        Console.WriteLine($"[MatchService] Calculating score for match ID: {matchId}");

        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
        {
            Console.WriteLine($"[MatchService] ERROR: Match ID {matchId} not found.");
            return "Match not found";
        }

        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var player1Points = points.Count(p => p.IsUserWinner);
        var player2Points = points.Count(p => !p.IsUserWinner);

        Console.WriteLine($"[MatchService] Current score: {player1Points} - {player2Points}");

        return $"{player1Points} - {player2Points}";
    }

    public async Task RegisterPointAsync(int matchId, int userId, string pointType, bool isUserWinner)
    {
        Console.WriteLine($"[MatchService] Registering point for match ID: {matchId}, won by {(isUserWinner ? "User" : "Opponent")}");

        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
        {
            Console.WriteLine($"[MatchService] ERROR: Match ID {matchId} not found. Point not registered.");
            return;
        }

        var point = new PointCreateDto
        {
            MatchId = matchId,
            PointType = pointType,
            NumberOfShots = 0,
            IsUserWinner = isUserWinner
        };

        await _pointRepository.AddPointAsync(point);
        Console.WriteLine("[MatchService] Point successfully registered.");
    }

    public async Task UndoLastPointAsync(int matchId)
    {
        Console.WriteLine($"[MatchService] Undoing last point for match ID: {matchId}");
        await _pointRepository.DeleteLastPointAsync(matchId);
    }

    // ----------- NEW: Implement required interface members -------------

    public async Task<PlayMatchDto?> GetPlayMatchDtoAsync(int matchId, int userId)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
            return null;

        var points = await _pointRepository.GetPointsByMatchIdAsync(matchId);
        var score = _scorekeepingService.CalculateScore(match, points);

        var displaySetIndices = new List<int>();
        var userSetGames = new List<string>();
        var opponentSetGames = new List<string>();
        string gameUserDisplay = "";
        string gameOpponentDisplay = "";

        for (int i = 0; i < score.SetScores.Count; i++)
        {
            var set = score.SetScores[i];
            bool isFinalSet = i == score.SetScores.Count - 1;
            bool isEmptyFinalSet = isFinalSet
                && set.Player1Games == 0
                && set.Player2Games == 0
                && score.MatchOver;

            if (!isEmptyFinalSet)
            {
                displaySetIndices.Add(i);
                userSetGames.Add(set.Player1Games.ToString());
                opponentSetGames.Add(set.Player2Games.ToString());
            }
        }

        var parts = score.CurrentGameScore.Split('-');
        if (parts.Length == 2)
        {
            gameUserDisplay = parts[0].Trim();
            gameOpponentDisplay = parts[1].Trim();
        }
        else
        {
            gameUserDisplay = gameOpponentDisplay = score.CurrentGameScore;
        }

        return new PlayMatchDto
        {
            Score = score,
            DisplaySetIndices = displaySetIndices,
            UserSetGames = userSetGames,
            OpponentSetGames = opponentSetGames,
            GameUserDisplay = gameUserDisplay,
            GameOpponentDisplay = gameOpponentDisplay
        };
    }

    public async Task<List<MatchHistoryDto>> GetMatchHistorySummariesAsync(int userId)
    {
        var matches = await _matchRepository.GetMatchesByUserIdAsync(userId);
        var summaries = new List<MatchHistoryDto>();

        foreach (var match in matches)
        {
            var points = await _pointRepository.GetPointsByMatchIdAsync(match.Id);
            var score = _scorekeepingService.CalculateScore(match, points);

            var formatted = string.Join(" ", score.SetScores
                .Where(s => s.Player1Games > 0 || s.Player2Games > 0)
                .Select(s => s.ToString()));

            summaries.Add(new MatchHistoryDto
            {
                Match = match,
                ScoreSummary = formatted,
                MatchOver = score.MatchOver
            });
        }

        return summaries;
    }
}

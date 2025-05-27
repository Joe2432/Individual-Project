using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPointRepository _pointRepository;
    private readonly IUserRepository _userRepository;

    public MatchService(
        IMatchRepository matchRepository,
        IPointRepository pointRepository,
        IUserRepository userRepository)
    {
        _matchRepository = matchRepository;
        _pointRepository = pointRepository;
        _userRepository = userRepository;
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
}

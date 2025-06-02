public interface IMatchService
{
    Task<int> CreateMatchAsync(MatchDto matchDto);
    Task DeleteMatchAsync(int matchId);
    Task<MatchDto?> GetMatchByIdAsync(int matchId);
    Task<List<MatchDto>> GetUserMatchesAsync(int userId);
    Task<string> GetScoreDisplayAsync(int matchId, int userId);
    Task RegisterPointAsync(int matchId, int userId, string pointType, bool isUserWinner);
    Task UndoLastPointAsync(int matchId);
    Task<MatchDto?> GetMatchForPlayingAsync(int matchId, int userId);
    Task<List<MatchDto>> GetMatchHistorySummariesAsync(
        int userId,
        string? name,
        string? type,
        string? surface,
        DateTime? date,
        DateTime? dateFrom,
        DateTime? dateTo);

    Task UpdateInitialServerAsync(int matchId, string initialServer); // <-- Add this line
}

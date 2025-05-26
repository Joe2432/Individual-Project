public interface IMatchService
{
    Task<int> CreateMatchAsync(MatchCreateDto dto);
    Task<List<MatchCreateDto>> GetUserMatchesAsync(int userId);
    Task<string> GetScoreDisplayAsync(int matchId, int userId);
    Task RegisterPointAsync(int matchId, int userId, string pointType); // Ensure this matches
    Task UndoLastPointAsync(int matchId);
}

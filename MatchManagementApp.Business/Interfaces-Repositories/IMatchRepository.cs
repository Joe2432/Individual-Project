public interface IMatchRepository
{
    Task<int> CreateMatchAsync(MatchDto dto);
    Task<MatchDto?> GetMatchByIdAsync(int id);
    Task DeleteMatchAsync(int matchId);
    Task<List<MatchDto>> GetMatchesByUserAsync(int userId);
    Task<List<MatchDto>> SearchMatchesAsync(int userId, string? type, string? surface, string? name, DateTime? start, DateTime? end, DateTime? today);
}
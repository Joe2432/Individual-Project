public interface IMatchRepository
{
    Task<int> CreateMatchAsync(MatchDto dto);
    Task<List<MatchDto>> GetMatchesByUserIdAsync(int userId);
    Task<MatchDto?> GetMatchByIdAsync(int matchId);
}

public interface IMatchRepository
{
    Task<int> CreateMatchAsync(MatchDto match);
    Task<MatchDto?> GetMatchByIdAsync(int matchId);
    Task<List<MatchDto>> GetMatchesByUserIdAsync(int userId);
    Task DeleteMatchAsync(int matchId);
    Task UpdateInitialServerAsync(int matchId, string initialServer);

}

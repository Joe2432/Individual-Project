public interface IMatchRepository
{
    Task<int> CreateMatchAsync(MatchEntity match);
    Task<List<MatchEntity>> GetMatchesByUserIdAsync(int userId);
    Task<MatchEntity?> GetMatchByIdAsync(int matchId);
    Task UpdateMatchAsync(MatchEntity match);
}
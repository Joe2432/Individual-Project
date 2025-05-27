public interface IMatchRepository
{
    Task<int> CreateMatchAsync(MatchCreateDto dto);
    Task<List<MatchReadDto>> GetMatchesByUserIdAsync(int userId);
    Task<MatchReadDto?> GetMatchByIdAsync(int matchId);
}

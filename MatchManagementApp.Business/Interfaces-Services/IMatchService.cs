public interface IMatchService
{
    Task<int> CreateMatchAsync(MatchCreateDto dto);
    Task<List<MatchReadDto>> GetUserMatchesAsync(int userId);
    Task<string> GetScoreDisplayAsync(int matchId, int userId);
    Task RegisterPointAsync(int matchId, int userId, string pointType, bool isUserWinner);
    Task UndoLastPointAsync(int matchId);
}

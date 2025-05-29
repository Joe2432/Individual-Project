public interface IMatchService
{
    Task<int> CreateMatchAsync(MatchDto dto);
    Task<List<MatchDto>> GetUserMatchesAsync(int userId);
    Task<string> GetScoreDisplayAsync(int matchId, int userId);
    Task RegisterPointAsync(int matchId, int userId, string pointType, bool isUserWinner);
    Task UndoLastPointAsync(int matchId);
    Task<PlayMatchDto?> GetPlayMatchDtoAsync(int matchId, int userId);
    Task<List<MatchHistoryDto>> GetMatchHistorySummariesAsync(
        int userId,
        string? name = null,
        string? type = null,
        string? surface = null,
        DateTime? date = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null
    );

}

public class MatchRepository : IMatchRepository
{
    private static readonly List<MatchEntity> _matches = new();

    public Task<int> CreateMatchAsync(MatchEntity match)
    {
        _matches.Add(match);
        return Task.FromResult(match.Id);
    }

    public Task<List<MatchEntity>> GetMatchesByUserIdAsync(int userId)
    {
        var result = _matches.Where(m => m.CreatedByUserId == userId).ToList();
        return Task.FromResult(result);
    }

    public Task<MatchEntity?> GetMatchByIdAsync(int matchId)
    {
        var match = _matches.FirstOrDefault(m => m.Id == matchId);
        return Task.FromResult(match);
    }

    public Task UpdateMatchAsync(MatchEntity match)
    {
        var index = _matches.FindIndex(m => m.Id == match.Id);
        if (index >= 0)
            _matches[index] = match;
        return Task.CompletedTask;
    }
}
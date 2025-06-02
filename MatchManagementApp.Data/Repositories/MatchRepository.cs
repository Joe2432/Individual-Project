using Microsoft.EntityFrameworkCore;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateMatchAsync(MatchDto dto)
    {
        var entity = dto.ToEntity();
        _context.Matches.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<List<MatchDto>> GetMatchesByUserIdAsync(int userId)
    {
        var entities = await _context.Matches
            .Where(m => m.CreatedByUserId == userId)
            .ToListAsync();

        return entities.Select(m => m.ToDto()).ToList();
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        var entity = await _context.Matches
            .FirstOrDefaultAsync(m => m.Id == matchId);

        return entity?.ToDto();
    }

    public async Task DeleteMatchAsync(int matchId)
    {
        var entity = await _context.Matches.FindAsync(matchId);
        if (entity != null)
        {
            _context.Matches.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    public async Task UpdateInitialServerAsync(int matchId, string initialServer)
    {
        var match = await _context.Matches.FindAsync(matchId);
        if (match != null)
        {
            match.InitialServer = initialServer;
            await _context.SaveChangesAsync();
        }
    }

}

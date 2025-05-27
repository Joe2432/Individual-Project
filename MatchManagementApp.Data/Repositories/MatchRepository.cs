using Microsoft.EntityFrameworkCore;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateMatchAsync(MatchEntity match)
    {
        _context.Matches.Add(match);
        await _context.SaveChangesAsync();
        return match.Id; // EF will auto-generate this
    }

    public async Task<List<MatchEntity>> GetMatchesByUserIdAsync(int userId)
    {
        return await _context.Matches
            .Where(m => m.CreatedByUserId == userId)
            .ToListAsync();
    }

    public async Task<MatchEntity?> GetMatchByIdAsync(int matchId)
    {
        return await _context.Matches
            .Include(m => m.Points)
            .FirstOrDefaultAsync(m => m.Id == matchId);
    }

    public async Task UpdateMatchAsync(MatchEntity match)
    {
        _context.Matches.Update(match);
        await _context.SaveChangesAsync();
    }
}

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

    public async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        var entity = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
        return entity?.ToDto();
    }

    public async Task DeleteMatchAsync(int matchId)
    {
        var match = await _context.Matches.FindAsync(matchId);
        if (match != null)
        {
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<MatchDto>> GetMatchesByUserAsync(int userId)
    {
        var matches = await _context.Matches
            .Where(m => m.CreatedByUserId == userId)
            .ToListAsync();

        return matches.Select(m => m.ToDto()).ToList();
    }

    public async Task<List<MatchDto>> SearchMatchesAsync(int userId, string? type, string? surface, string? name, DateTime? start, DateTime? end, DateTime? today)
    {
        var query = _context.Matches.AsQueryable();

        query = query.Where(m => m.CreatedByUserId == userId);

        if (!string.IsNullOrEmpty(type))
            query = query.Where(m => m.MatchType == type);

        if (!string.IsNullOrEmpty(surface))
            query = query.Where(m => m.Surface == surface);

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(m =>
                (m.FirstOpponentName != null && m.FirstOpponentName.Contains(name)) ||
                (m.SecondOpponentName != null && m.SecondOpponentName.Contains(name)) ||
                (m.PartnerName != null && m.PartnerName.Contains(name)));
        }

        if (start.HasValue)
            query = query.Where(m => m.MatchDate >= start.Value);

        if (end.HasValue)
            query = query.Where(m => m.MatchDate <= end.Value);

        if (today.HasValue)
            query = query.Where(m => m.MatchDate.Date == today.Value.Date);

        var result = await query.ToListAsync();
        return result.Select(m => m.ToDto()).ToList();
    }
}

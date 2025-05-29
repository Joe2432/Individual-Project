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
        var entity = new MatchEntity(
            dto.CreatedByUserId,
            dto.MatchType,
            dto.FirstOpponentName,
            dto.NrSets,
            dto.FinalSetType,
            dto.GameFormat,
            dto.Surface,
            dto.PartnerName,
            dto.SecondOpponentName
        );

        _context.Matches.Add(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<List<MatchDto>> GetMatchesByUserIdAsync(int userId)
    {
        return await _context.Matches
            .Where(m => m.CreatedByUserId == userId)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                MatchType = m.MatchType,
                PartnerName = m.PartnerName,
                FirstOpponentName = m.FirstOpponentName,
                SecondOpponentName = m.SecondOpponentName,
                Surface = m.Surface,
                NrSets = m.NrSets,
                FinalSetType = m.FinalSetType,
                GameFormat = m.GameFormat,
                MatchDate = m.MatchDate
            })
            .ToListAsync();
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        return await _context.Matches
            .Where(m => m.Id == matchId)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                MatchType = m.MatchType,
                PartnerName = m.PartnerName,
                FirstOpponentName = m.FirstOpponentName,
                SecondOpponentName = m.SecondOpponentName,
                Surface = m.Surface,
                NrSets = m.NrSets,
                FinalSetType = m.FinalSetType,
                GameFormat = m.GameFormat,
                MatchDate = m.MatchDate
            })
            .FirstOrDefaultAsync();
    }
}

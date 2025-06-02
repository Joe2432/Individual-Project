using Microsoft.EntityFrameworkCore;

public class PointRepository : IPointRepository
{
    private readonly AppDbContext _context;

    public PointRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PointDto>> GetPointsByMatchIdAsync(int matchId)
    {
        var entities = await _context.Points
            .Where(p => p.MatchId == matchId)
            .ToListAsync();

        return entities.Select(p => p.ToReadDto()).ToList();
    }


    public async Task AddPointAsync(PointDto point)
    {
        var entity = point.ToEntity();
        _context.Points.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLastPointAsync(int matchId)
    {
        var lastPoint = await _context.Points
            .Where(p => p.MatchId == matchId)
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        if (lastPoint != null)
        {
            _context.Points.Remove(lastPoint);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;

public class PointRepository : IPointRepository
{
    private readonly AppDbContext _context;

    public PointRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PointEntity>> GetPointsByMatchIdAsync(int matchId)
    {
        return await _context.Points
            .Where(p => p.MatchId == matchId)
            .ToListAsync();
    }

    public async Task<PointEntity?> GetPointByIdAsync(int pointId)
    {
        return await _context.Points.FindAsync(pointId);
    }

    public async Task AddPointAsync(PointEntity point)
    {
        _context.Points.Add(point);
        await _context.SaveChangesAsync();
    }


    public async Task DeletePointAsync(PointEntity point)
    {
        _context.Points.Remove(point);
        await _context.SaveChangesAsync();
    }
}

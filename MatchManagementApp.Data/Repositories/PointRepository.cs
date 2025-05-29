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
        return await _context.Points
            .Where(p => p.MatchId == matchId)
            .Select(p => new PointDto
            {
                PointType = p.WinningMethod,
                NumberOfShots = p.NrShots,
                IsUserWinner = p.WinnerLabel == "User"
            })
            .ToListAsync();
    }

    public async Task<PointEntity?> GetPointByIdAsync(int pointId)
    {
        return await _context.Points.FindAsync(pointId);
    }

    public async Task AddPointAsync(PointDto dto)
    {
        var winnerLabel = dto.IsUserWinner ? "User" : "Opponent";

        var entity = new PointEntity(
            dto.MatchId,
            winnerLabel,
            dto.PointType,
            dto.NumberOfShots
        );

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

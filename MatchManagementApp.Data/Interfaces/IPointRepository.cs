public interface IPointRepository
{
    Task AddPointAsync(PointEntity point);
    Task<List<PointEntity>> GetPointsByMatchIdAsync(int matchId);
    Task DeletePointAsync(PointEntity point);
}

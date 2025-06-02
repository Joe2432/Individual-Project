public interface IPointRepository
{
    Task AddPointAsync(PointDto dto);
    Task<List<PointDto>> GetPointsByMatchIdAsync(int matchId);
    Task DeleteLastPointAsync(int matchId);

}

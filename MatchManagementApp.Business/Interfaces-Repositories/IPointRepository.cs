public interface IPointRepository
{
    Task AddPointAsync(PointCreateDto dto);
    Task<List<PointReadDto>> GetPointsByMatchIdAsync(int matchId);
    Task DeleteLastPointAsync(int matchId);

}

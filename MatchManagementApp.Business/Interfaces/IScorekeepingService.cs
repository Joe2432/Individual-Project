public interface IScorekeepingService
{
    MatchScoreDto CalculateScore(MatchEntity match, IEnumerable<PointEntity> points);
}

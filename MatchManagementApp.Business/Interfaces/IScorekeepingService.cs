public interface IScorekeepingService
{
    MatchScoreDto CalculateScore(MatchEntity match, List<PointEntity> points);
}

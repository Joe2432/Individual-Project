public interface IScorekeepingService
{
    MatchScoreDto CalculateScore(MatchReadDto match, IEnumerable<PointReadDto> points);
}

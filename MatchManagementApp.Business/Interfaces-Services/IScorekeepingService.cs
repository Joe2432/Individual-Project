using System.Collections.Generic;

public interface IScorekeepingService
{
    MatchScoreDto CalculateScore(MatchDto match, IEnumerable<PointDto> points);
}

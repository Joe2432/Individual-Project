using System.Collections.Generic;

public interface IScorekeepingService
{
    MatchDto CalculateScore(MatchDto match, IEnumerable<PointDto> points);
}

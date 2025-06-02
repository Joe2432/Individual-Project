
public interface IAnalysisService
{
    Task<AnalysisDto> GetAnalysisAsync(int matchId, int currentUserId);
}
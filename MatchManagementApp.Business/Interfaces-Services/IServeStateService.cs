public interface IServeStateService
{
    ServeState GetServeState(MatchDto match, List<PointDto> points);
}

public class ServeState
{
    public string CurrentServer { get; set; }
    public bool IsFirstServe { get; set; }
}

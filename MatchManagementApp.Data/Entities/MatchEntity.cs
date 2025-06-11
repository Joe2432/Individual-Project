public class MatchEntity
{
    public int Id { get; private set; }

    public int CreatedByUserId { get; private set; }
    public string MatchType { get; private set; }
    public string? PartnerName { get; private set; }
    public string FirstOpponentName { get; private set; }
    public string? SecondOpponentName { get; private set; }
    public int NrSets { get; private set; }
    public string FinalSetType { get; private set; }
    public string GameFormat { get; private set; }
    public string Surface { get; private set; }
    public DateTime MatchDate { get; private set; } = DateTime.UtcNow;

    public string InitialServer { get; set; }

    public UserEntity User { get; private set; } = null!;
    public ICollection<PointEntity> Points { get; private set; } = new List<PointEntity>();
    public MatchEntity(
     int createdByUserId,
     string matchType,
     string firstOpponentName,
     int nrSets,
     string finalSetType,
     string gameFormat,
     string surface,
     string initialServer,
     string? partnerName,
     string? secondOpponentName
 )
    {
        CreatedByUserId = createdByUserId;
        MatchType = matchType;
        FirstOpponentName = firstOpponentName;
        NrSets = nrSets;
        FinalSetType = finalSetType;
        GameFormat = gameFormat;
        Surface = surface;
        InitialServer = initialServer;
        PartnerName = partnerName;
        SecondOpponentName = secondOpponentName;
        MatchDate = DateTime.UtcNow;
    }
}

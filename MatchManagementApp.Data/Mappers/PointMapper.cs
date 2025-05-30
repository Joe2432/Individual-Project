public static class PointMapper
{
    public static PointDto ToReadDto(this PointEntity entity)
    {
        return new PointDto
        {
            PointType = entity.WinningMethod,
            NumberOfShots = entity.NrShots,
            IsUserWinner = entity.WinnerLabel == "User"
        };
    }

    public static PointEntity ToEntity(this PointDto dto)
    {
        var winnerLabel = dto.IsUserWinner ? "User" : "Opponent";
        return new PointEntity(dto.MatchId, winnerLabel, dto.PointType, dto.NumberOfShots);
    }

}

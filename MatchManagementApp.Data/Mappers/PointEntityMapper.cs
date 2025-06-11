public static class PointEntityMapper
{
    public static PointDto ToReadDto(this PointEntity entity)
    {
        return new PointDto
        {
            PointType = entity.WinningMethod,
            NumberOfShots = entity.NrShots,
            IsUserWinner = entity.WinnerLabel == "User",
            IsFirstServe = entity.IsFirstServe
        };
    }


    public static PointEntity ToEntity(this PointDto dto)
    {
        var winnerLabel = dto.IsUserWinner ? "User" : "Opponent";
        return new PointEntity(dto.MatchId, winnerLabel, dto.PointType, dto.NumberOfShots, dto.IsFirstServe);
    }

}

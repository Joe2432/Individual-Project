public static class PointMapper
{
    public static PointReadDto ToReadDto(this PointEntity entity)
    {
        return new PointReadDto
        {
            PointType = entity.WinningMethod,
            NumberOfShots = entity.NrShots,
            IsUserWinner = entity.WinnerLabel == "User"
        };
    }

    public static PointEntity ToEntity(this PointCreateDto dto)
    {
        var winnerLabel = dto.IsUserWinner ? "User" : "Opponent";
        return new PointEntity(dto.MatchId, winnerLabel, dto.PointType, dto.NumberOfShots);
    }
}

public static class PointMapper
{
    public static PointReadDto ToReadDto(this PointEntity entity, int userId)
    {
        return new PointReadDto
        {
            PointType = entity.WinningMethod,
            NumberOfShots = entity.NrShots,
            IsUserWinner = entity.WinnerId == userId
        };
    }

    public static PointEntity ToEntity(this PointCreateDto dto, int userId)
    {
        var winnerId = dto.IsUserWinner ? userId : -1;
        return new PointEntity(dto.MatchId, winnerId, dto.PointType, dto.NumberOfShots);
    }
}

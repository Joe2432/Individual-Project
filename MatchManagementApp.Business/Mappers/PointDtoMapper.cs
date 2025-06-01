public static class PointDtoMapper
{
    public static Point ToModel(PointDto dto)
    {
        return new Point(
            matchId: dto.MatchId,
            pointType: dto.PointType ?? string.Empty,
            numberOfShots: dto.NumberOfShots,
            isUserWinner: dto.IsUserWinner
        );
    }

    public static PointDto ToDto(Point model)
    {
        return new PointDto
        {
            MatchId = model.MatchId,
            PointType = model.PointType,
            NumberOfShots = model.NumberOfShots,
            IsUserWinner = model.IsUserWinner
        };
    }
}

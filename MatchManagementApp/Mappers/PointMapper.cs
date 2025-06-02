public static class PointMapper
{
    public static PointDto ToDto(PointViewModel vm)
    {
        return new PointDto
        {
            MatchId = vm.MatchId,
            PointType = vm.PointType,
            NumberOfShots = vm.NumberOfShots,
            IsUserWinner = vm.IsUserWinner,
            IsFirstServe = vm.IsFirstServe
        };
    }

    public static PointViewModel ToViewModel(PointDto dto)
    {
        return new PointViewModel
        {
            MatchId = dto.MatchId,
            PointType = dto.PointType ?? string.Empty,
            NumberOfShots = dto.NumberOfShots,
            IsUserWinner = dto.IsUserWinner,
            IsFirstServe = dto.IsFirstServe
        };
    }
}

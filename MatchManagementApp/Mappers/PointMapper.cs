public static class PointMapper
{
    public static PointCreateDto ToCreateDto(PointInputViewModel vm)
    {
        return new PointCreateDto
        {
            MatchId = vm.MatchId,
            PointType = vm.PointType,
            IsUserWinner = vm.IsUserWinner,
            NumberOfShots = vm.NumberOfShots
        };
    }
}


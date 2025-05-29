public static class PointMapper
{
    public static PointDto ToCreateDto(PointInputViewModel vm)
    {
        return new PointDto
        {
            MatchId = vm.MatchId,
            PointType = vm.PointType,
            IsUserWinner = vm.IsUserWinner,
            NumberOfShots = vm.NumberOfShots
        };
    }
}


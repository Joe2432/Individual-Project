public static class SetScoreDtoMapper
{
    public static SetScore ToModel(SetScoreDto dto)
    {
        return new SetScore(
            userGames: dto.Player1Games,
            opponentGames: dto.Player2Games,
            tiebreakScore: dto.TiebreakScore
        );
    }

    public static SetScoreDto ToDto(SetScore model)
    {
        return new SetScoreDto
        {
            Player1Games = model.UserGames,
            Player2Games = model.OpponentGames,
            TiebreakScore = model.TiebreakScore
        };
    }
}

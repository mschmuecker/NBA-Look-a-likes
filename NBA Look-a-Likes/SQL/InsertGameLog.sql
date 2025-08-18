INSERT INTO GameStats (
    gameId,
    personId,
    minutes,
    points,
    reboundsTotal,
    assists
)
VALUES (
    @gameId,
    @personId,
    @minutes,
    @points,
    @rebounds,
    @assists
);
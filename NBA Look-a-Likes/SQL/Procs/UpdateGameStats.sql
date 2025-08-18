SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.UpdateGameStats
    @GameId int,
    @PersonId int,
    @Minutes float,
    @Points float,
    @Rebounds float,
    @Assists float
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.GameStats
    SET
        numMinutes    = @Minutes,
        points        = @Points,
        reboundsTotal = @Rebounds,
        assists       = @Assists
    WHERE gameId = @GameId
      AND personId = @PersonId;

END
GO

SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.InsertGameStats
    @GameId int,
    @PersonId int,
    @Minutes float,
    @Points float,
    @Rebounds float,
    @Assists float,
    @Home bit
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.GameStats (
        gameId,
        personId,
        numMinutes,
        points,
        reboundsTotal,
        assists,
        home
    )
    VALUES (
        @GameId,
        @PersonId,
        @Minutes,
        @Points,
        @Rebounds,
        @Assists,
        @Home
       
    );
END
GO

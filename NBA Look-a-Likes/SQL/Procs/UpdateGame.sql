SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.UpdateGame
    @GameId int, 
    @HomeScore tinyint,
    @AwayScore tinyint,
    @Winner int,
    @GameType nvarchar(50),
    @Attendance float
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Game
    SET
        homeScore     = @HomeScore,
        awayScore     = @AwayScore,
        winner        = @Winner,
        gameType      = @GameType,
        attendance    = @Attendance
    WHERE gameId = @GameId;
END
GO

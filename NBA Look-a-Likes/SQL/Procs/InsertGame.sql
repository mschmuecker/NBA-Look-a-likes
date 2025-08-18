SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.InsertGame
    @GameId int, 
    @GameDate datetime2(7),
    @HomeTeamId int,
    @HomeCity nvarchar(50),
    @HomeName nvarchar(50),
    @AwayTeamId int,
    @AwayCity nvarchar(50),
    @AwayName nvarchar(50),
    @HomeScore tinyint,
    @AwayScore tinyint,
    @Winner int,
    @GameType nvarchar(50),
    @Attendance float
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Game (
        gameId,
        gameDate,
        homeTeamId,
        homeTeamCity,
        homeTeamName,
        awayTeamId,
        awayTeamCity,
        awayTeamName,
        homeScore,
        awayScore,
        winner,
        gameType,
        attendance
    )
    OUTPUT INSERTED.gameId
    VALUES (
        @GameId,
        @GameDate,
        @HomeTeamId,
        @HomeCity,
        @HomeName,
        @AwayTeamId,
        @AwayCity,
        @AwayName,
        @HomeScore,
        @AwayScore,
        @Winner,
        @GameType,
        @Attendance
    );
END
GO

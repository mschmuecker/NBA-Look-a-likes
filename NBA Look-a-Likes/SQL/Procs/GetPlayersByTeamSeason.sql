SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetPlayersByTeamSeason
    @TeamId int,
    @Season smallint
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT Players.*
    FROM dbo.PlayedFor
    JOIN Players 
        ON Players.personId = PlayedFor.playerId
    WHERE @TeamId = teamId
      AND @Season BETWEEN startSeason AND endSeason;
END
GO

SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetTeamsPlayedForByPlayer
    @PlayerId int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.PlayedFor
    WHERE @PlayerId = playerId;
END
GO

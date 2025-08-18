SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetGameStatsByGameId
    @GameId int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.GameStats
    WHERE @GameId = GameId;
END
GO

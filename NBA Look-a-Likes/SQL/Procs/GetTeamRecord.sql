SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetTeamRecord_Dedup
    @TeamId int
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH TeamGames AS (
        SELECT DISTINCT
            g.gameId,
            g.winner
        FROM dbo.Game AS g
        WHERE @TeamId IN (g.homeTeamId, g.awayTeamId)
    )
    SELECT
        @TeamId AS TeamId,
        COUNT(*) AS GamesPlayed,
        SUM(CASE WHEN winner = @TeamId THEN 1 ELSE 0 END) AS Wins,
        SUM(CASE WHEN winner IS NOT NULL AND winner <> @TeamId THEN 1 ELSE 0 END) AS Losses
    FROM TeamGames;
END
GO

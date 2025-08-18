SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetTeamRecordBySeason
    @TeamId int
AS
BEGIN
    SET NOCOUNT ON;

    WITH TeamGames AS (
        /* One row per game the team played, with points for/against */
        SELECT DISTINCT
            g.gameId,
            g.gameDate,
            g.winner,
            CASE WHEN g.homeTeamId = @TeamId THEN 1 ELSE 0 END AS IsHome,
            CASE WHEN g.homeTeamId = @TeamId THEN g.homeScore ELSE g.awayScore END AS Points,
            CASE WHEN g.homeTeamId = @TeamId THEN g.awayScore ELSE g.homeScore END AS PointsAgainst
        FROM dbo.Game AS g
        WHERE @TeamId IN (g.homeTeamId, g.awayTeamId)
    )
    SELECT
        @TeamId AS TeamId,
        CASE 
            WHEN MONTH(gameDate) >= 10 THEN YEAR(gameDate)
            ELSE YEAR(gameDate) - 1
        END AS Season,
        COUNT(*) AS GamesPlayed,
        SUM(CASE WHEN winner = @TeamId THEN 1 ELSE 0 END)  AS Wins,
        SUM(CASE WHEN winner IS NOT NULL AND winner <> @TeamId THEN 1 ELSE 0 END) AS Losses,
        AVG(CAST(Points AS float))  AS PointsPerGame,
        AVG(CAST(PointsAgainst AS float))  AS PointsAgainstPerGame
   
    FROM TeamGames
    GROUP BY
        CASE 
            WHEN MONTH(gameDate) >= 10 THEN YEAR(gameDate)
            ELSE YEAR(gameDate) - 1
        END
    ORDER BY Season DESC;
END
GO

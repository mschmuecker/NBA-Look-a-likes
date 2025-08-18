SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetPlayerSeasonStats
    @PlayerId int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CASE 
            WHEN MONTH(GameDate) >= 10 THEN YEAR(GameDate)
            ELSE YEAR(GameDate) - 1
        END AS SeasonYear,
        COUNT(*) AS Games,
        SUM(Points) AS TotalPoints,
        AVG(CAST(Points AS FLOAT)) AS PPG,
        SUM(ReboundsTotal) AS TotalRebounds,
        AVG(CAST(ReboundsTotal AS FLOAT)) AS RPG,
        SUM(Assists) AS TotalAssists,
        AVG(CAST(Assists AS FLOAT)) AS APG,
        AVG(CAST(GS.fieldGoalsPercentage AS FLOAT)) AS FGPercentage,
        AVG(CAST(GS.threePointersPercentage AS FLOAT)) AS ThreePPercentage,
        AVG(CAST(GS.turnovers AS FLOAT)) AS Turnovers,
        AVG(CAST(GS.freeThrowsPercentage AS FLOAT)) AS FT,
        AVG(CAST(GS.steals AS FLOAT)) AS SPG,
        AVG(CAST(GS.blocks AS FLOAT)) AS BPG
    FROM GameStats GS
    WHERE personId = @PlayerId
    GROUP BY
        CASE 
            WHEN MONTH(GameDate) >= 10 THEN YEAR(GameDate)
            ELSE YEAR(GameDate) - 1
        END
    ORDER BY SeasonYear DESC;
END
GO

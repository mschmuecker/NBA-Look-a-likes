SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetStatLeaders
    @Stat varchar(10),
    @Top int = 5
AS
BEGIN
    SET NOCOUNT ON;

    WITH StatLeaders AS (
        SELECT 
            GS.personId,
            CASE 
                WHEN @Stat = 'PPG' THEN SUM(GS.points)
                WHEN @Stat = 'RPG' THEN SUM(GS.reboundsTotal)
                WHEN @Stat = 'APG' THEN SUM(GS.assists)
                WHEN @Stat = 'TOPG' THEN SUM(GS.turnovers)
                WHEN @Stat = '3PM' THEN SUM(GS.threePointersMade)
                WHEN @Stat = 'FTM' THEN SUM(GS.freeThrowsMade)
                ELSE 0
            END AS StatValue
        FROM GameStats GS
        GROUP BY GS.personId
    )
    SELECT TOP (@Top)
        P.*, -- all columns from Players
        S.StatValue
    FROM StatLeaders S
    JOIN Players P ON P.personId = S.personId
    ORDER BY S.StatValue DESC;
END
GO

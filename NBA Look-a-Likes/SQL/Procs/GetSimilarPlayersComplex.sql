SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetSimilarPlayersByWeightedStats
    @PlayerId int,
    @UsePPG bit,
    @WeightPPG float,
    @UseRPG bit,
    @WeightRPG float,
    @UseAPG bit,
    @WeightAPG float,
    @UseTOPG bit,
    @WeightTOPG float,
    @UseThreePM bit,
    @WeightThreePM float,
    @UseFTM bit,
    @WeightFTM float,
    @UseSTL bit,
    @WeightSTL float,
    @UseBLK bit,
    @WeightBLK float,
    @Top int = 20
AS
BEGIN
    SET NOCOUNT ON;

    WITH TargetStats AS (
        SELECT 
            AVG(CAST(GS.points AS FLOAT)) AS PPG,
            AVG(CAST(GS.reboundsTotal AS FLOAT)) AS RPG,
            AVG(CAST(GS.assists AS FLOAT)) AS APG,
            AVG(CAST(GS.turnovers AS FLOAT)) AS TOPG,
            AVG(CAST(GS.threePointersMade AS FLOAT)) AS ThreePM,
            AVG(CAST(GS.freeThrowsMade AS FLOAT)) AS FTM,
            AVG(CAST(GS.steals AS FLOAT)) AS STL,
            AVG(CAST(GS.blocks AS FLOAT)) AS BLK
        FROM GameStats GS
        WHERE GS.personId = @PlayerId
    ),
    CareerAverages AS (
        SELECT 
            P.personId,
            P.firstName,
            P.lastName,
            AVG(CAST(GS.points AS FLOAT)) AS PPG,
            AVG(CAST(GS.reboundsTotal AS FLOAT)) AS RPG,
            AVG(CAST(GS.assists AS FLOAT)) AS APG,
            AVG(CAST(GS.turnovers AS FLOAT)) AS TOPG,
            AVG(CAST(GS.threePointersMade AS FLOAT)) AS ThreePM,
            AVG(CAST(GS.freeThrowsMade AS FLOAT)) AS FTM,
            AVG(CAST(GS.steals AS FLOAT)) AS STL,
            AVG(CAST(GS.blocks AS FLOAT)) AS BLK
        FROM Players P
        JOIN GameStats GS 
            ON P.personId = GS.personId
        WHERE P.personId != @PlayerId
        GROUP BY P.personId, P.firstName, P.lastName
    ),
    Scored AS (
    SELECT 
        ca.personId,
        ca.firstName,
        ca.lastName,
        ca.PPG,
        ca.RPG,
        ca.APG,
        ca.TOPG,
        ca.ThreePM,
        ca.FTM,
        ca.STL,
        ca.BLK,
        (
            IIF(@UsePPG = 1, POWER((ca.PPG - ts.PPG) * @WeightPPG, 2), 0) +
            IIF(@UseRPG = 1, POWER((ca.RPG - ts.RPG) * @WeightRPG, 2), 0) +
            IIF(@UseAPG = 1, POWER((ca.APG - ts.APG) * @WeightAPG, 2), 0) +
            IIF(@UseTOPG = 1, POWER((ca.TOPG - ts.TOPG) * @WeightTOPG, 2), 0) +
            IIF(@UseThreePM = 1, POWER((ca.ThreePM - ts.ThreePM) * @WeightThreePM, 2), 0) +
            IIF(@UseFTM = 1, POWER((ca.FTM - ts.FTM) * @WeightFTM, 2), 0) +
            IIF(@UseSTL = 1, POWER((ca.STL - ts.STL) * @WeightSTL, 2), 0) +
            IIF(@UseBLK = 1, POWER((ca.BLK - ts.BLK) * @WeightBLK, 2), 0)
        ) AS StatDistance
    FROM CareerAverages ca
    CROSS JOIN TargetStats ts
    )
    SELECT *
    FROM Scored
    WHERE StatDistance IS NOT NULL              -- <-- filter out NULLs
    ORDER BY StatDistance ASC
    OFFSET 0 ROWS FETCH NEXT @Top ROWS ONLY;
END
GO

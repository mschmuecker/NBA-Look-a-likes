SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[GetSimilarPlayersByStats]
    @PlayerId int,
    @Top int = 5
AS
BEGIN
    SET NOCOUNT ON;

    WITH TargetStats AS (
        SELECT 
            AVG(CAST(GS.points AS FLOAT)) AS PPG,
            AVG(CAST(GS.reboundsTotal AS FLOAT)) AS RPG,
            AVG(CAST(GS.assists AS FLOAT)) AS APG
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
            AVG(CAST(GS.assists AS FLOAT)) AS APG
        FROM Players P
        JOIN GameStats GS 
            ON P.personId = GS.personId
        WHERE P.personId != @PlayerId -- exclude self
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
            POWER(ca.PPG - ts.PPG, 2)
          + POWER(ca.RPG - ts.RPG, 2)
          + POWER(ca.APG - ts.APG, 2) AS StatDistance
        FROM CareerAverages ca
        CROSS JOIN TargetStats ts
    )
 SELECT 
        personId,
        firstName,
        lastName,
        PPG,
        RPG,
        APG,
        StatDistance
    FROM Scored
    WHERE StatDistance IS NOT NULL              -- <-- filter out NULLs
    ORDER BY StatDistance ASC
    OFFSET 0 ROWS FETCH NEXT @Top ROWS ONLY;
END
GO
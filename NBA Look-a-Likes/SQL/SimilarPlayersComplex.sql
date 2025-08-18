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
    WHERE GS.personId = @playerId
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
    JOIN GameStats GS ON P.personId = GS.personId
    WHERE P.personId != @playerId
    GROUP BY P.personId, P.firstName, P.lastName
)
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
        IIF(@usePPG = 1, POWER((ca.PPG - ts.PPG) * @weightPPG, 2), 0) +
        IIF(@useRPG = 1, POWER((ca.RPG - ts.RPG) * @weightRPG, 2), 0) +
        IIF(@useAPG = 1, POWER((ca.APG - ts.APG) * @weightAPG, 2), 0) +
        IIF(@useTOPG = 1, POWER((ca.TOPG - ts.TOPG) * @weightTOPG, 2), 0) +
        IIF(@useThreePM = 1, POWER((ca.ThreePM - ts.ThreePM) * @weightThreePM, 2), 0) +
        IIF(@useFTM = 1, POWER((ca.FTM - ts.FTM) * @weightFTM, 2), 0) +
        IIF(@useSTL = 1, POWER((ca.STL - ts.STL) * @weightSTL, 2), 0) +
        IIF(@useBLK = 1, POWER((ca.BLK - ts.BLK) * @weightBLK, 2), 0)
    ) AS StatDistance

FROM CareerAverages ca
CROSS JOIN TargetStats ts
ORDER BY StatDistance ASC
OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;
WITH TargetStats AS (
    SELECT 
        AVG(CAST(GS.points AS FLOAT)) AS PPG,
        AVG(CAST(GS.reboundsTotal AS FLOAT)) AS RPG,
        AVG(CAST(GS.assists AS FLOAT)) AS APG
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
        AVG(CAST(GS.assists AS FLOAT)) AS APG
    FROM Players P
    JOIN GameStats GS ON P.personId = GS.personId
    WHERE P.personId != @playerId -- exclude self
    GROUP BY P.personId, P.firstName, P.lastName
)

SELECT 
    ca.personId,
    ca.firstName,
    ca.lastName,
    ca.PPG,
    ca.RPG,
    ca.APG,
    (
        POWER(ca.PPG - ts.PPG, 2) +
        POWER(ca.RPG - ts.RPG, 2) +
        POWER(ca.APG - ts.APG, 2)
    ) AS StatDistance
FROM CareerAverages ca
CROSS JOIN TargetStats ts
ORDER BY StatDistance ASC
OFFSET 0 ROWS FETCH NEXT @top ROWS ONLY;
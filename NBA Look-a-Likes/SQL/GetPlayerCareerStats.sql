SELECT 
P.personId, P.firstName, P.lastName,
AVG(CAST(GS.points AS FLOAT)) AS PPG,
AVG(CAST(GS.reboundsTotal AS FLOAT)) AS RPG,
AVG(CAST(GS.assists AS FLOAT)) AS APG,
AVG(CAST(GS.fieldGoalsPercentage AS FLOAT)) AS FGPercentage,
AVG(CAST(GS.threePointersPercentage AS FLOAT)) AS ThreePPercentage,
AVG(CAST(GS.turnovers AS FLOAT)) AS Turnovers,
AVG(CAST(GS.freeThrowsPercentage AS FLOAT)) AS FT,
AVG(CAST(GS.steals AS FLOAT)) AS SPG,
AVG(CAST(GS.blocks AS FLOAT)) AS BPG
FROM Players P
JOIN GameStats GS ON P.personId = GS.personId
WHERE P.personId = @playerId
GROUP BY P.personId, P.firstName, P.lastName
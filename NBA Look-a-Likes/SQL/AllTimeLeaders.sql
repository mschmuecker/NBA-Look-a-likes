--this will pull the top 5 best in each stat PPG, Rebounds, Assists, Turnovers, Three Pointers Made, Free Throws Made
WITH StatLeaders AS (
    SELECT 
        GS.personId,
        CASE 
            WHEN @stat = 'PPG' THEN SUM(GS.points )
            WHEN @stat = 'RPG' THEN SUM(GS.reboundsTotal )
            WHEN @stat = 'APG' THEN SUM(GS.assists )
            WHEN @stat = 'TOPG' THEN SUM(GS.turnovers )
            WHEN @stat = '3PM' THEN SUM(GS.threePointersMade)
            WHEN @stat = 'FTM' THEN SUM(GS.freeThrowsMade)
            ELSE 0
        END AS StatValue
    FROM GameStats GS
    GROUP BY GS.personId
)

SELECT TOP 5
    P.*, -- all columns from Players
    S.StatValue
FROM StatLeaders S
JOIN Players P ON P.personId = S.personId
ORDER BY S.StatValue DESC;
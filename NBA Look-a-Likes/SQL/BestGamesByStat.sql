-- BestGamesByStat.sql
WITH LatestTeams AS (
    SELECT t.*
    FROM Team t
    JOIN (
        SELECT teamId, MAX(seasonActiveTill) AS MaxSeason
        FROM Team
        GROUP BY teamId
    ) latest ON t.teamId = latest.teamId AND t.seasonActiveTill = latest.MaxSeason
)

SELECT TOP 5
    GS.GameDate,
    CASE
        WHEN @stat = 'PPG' THEN GS.Points
        WHEN @stat = 'RPG' THEN GS.ReboundsTotal
        WHEN @stat = 'APG' THEN GS.Assists
        WHEN @stat = 'STL' THEN GS.steals
        WHEN @stat = 'BLK' THEN GS.blocks
        WHEN @stat = '3PM' THEN GS.threePointersMade
        WHEN @stat = 'FTM' THEN GS.freeThrowsMade
    END as 'StatValue',
    GS.home,
    GS.gameType,
    G.homeScore,
    G.awayScore,
    home.teamId AS homeId,
    away.teamId AS awayId

FROM GameStats GS
JOIN Game G ON G.gameId = GS.gameId
JOIN LatestTeams home ON G.hometeamid = home.teamid
JOIN LatestTeams away ON G.awayteamid = away.teamid

WHERE GS.PersonId = @playerId

ORDER BY
    CASE
        WHEN @stat = 'PPG' THEN GS.Points
        WHEN @stat = 'RPG' THEN GS.ReboundsTotal
        WHEN @stat = 'APG' THEN GS.Assists
        WHEN @stat = 'STL' THEN GS.steals
        WHEN @stat = 'BLK' THEN GS.blocks
        WHEN @stat = '3PM' THEN GS.threePointersMade
        WHEN @stat = 'FTM' THEN GS.freeThrowsMade
    END DESC;
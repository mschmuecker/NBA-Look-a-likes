SELECT DISTINCT Players.*

FROM dbo.PlayedFor
JOIN  Players on players.personId = PlayedFor.playerId

WHERE @teamId=teamId AND @season BETWEEN startSeason AND endSeason
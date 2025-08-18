SELECT *
FROM dbo.Game
WHERE CAST(@gameDate AS DATE) = CAST(gameDate AS DATE);
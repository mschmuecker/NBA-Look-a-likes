SELECT * 
FROM dbo.Team 
WHERE @teamId = teamId
ORDER BY seasonActiveTill DESC; --most recent first
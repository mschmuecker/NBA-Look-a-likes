SELECT *
FROM dbo.Team
WHERE @season BETWEEN seasonfounded AND seasonactivetill AND league='NBA' and teamCity != 'All-Star'

﻿SELECT DISTINCT teamId
FROM dbo.Team
WHERE seasonActiveTill>YEAR(GETDATE())
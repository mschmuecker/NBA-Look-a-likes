SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetTeamById
    @TeamId int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Team
    WHERE TeamId = @TeamId
    ORDER BY SeasonActiveTill DESC;
END
GO

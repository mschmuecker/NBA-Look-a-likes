SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetTeamsBySeason
    @Season int,
    @League nvarchar(50) = 'NBA'

AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Team
    WHERE @Season BETWEEN seasonFounded AND seasonActiveTill
      AND league = @League
      AND teamCity != 'All-Star';
END
GO

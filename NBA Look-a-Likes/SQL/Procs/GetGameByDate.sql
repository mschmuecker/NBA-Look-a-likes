SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetGameByDate
    @GameDate date
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Game
    WHERE CAST(@GameDate AS date) = CAST(GameDate AS date);
END
GO

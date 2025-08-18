SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetPlayerById
    @PlayerId int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * 
    FROM Players 
    WHERE personId = @PlayerId;
END
GO

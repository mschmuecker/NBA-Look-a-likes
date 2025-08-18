SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.UpdatePlayer
    @PlayerId int,
    @FirstName nvarchar(50),
    @LastName nvarchar(50),
    @College nvarchar(50),
    @IsGuard bit,
    @IsForward bit,
    @IsCenter bit,
    @DraftYear float,
    @DraftRound float,
    @DraftNumber float,
    @Height float,
    @Weight float
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Players
    SET FirstName   = @FirstName,
        LastName    = @LastName,
        lastAttended = @College,
        guard       = @IsGuard,
        forward     = @IsForward,
        center      = @IsCenter,
        draftYear   = @DraftYear,
        draftRound  = @DraftRound,
        draftNumber = @DraftNumber,
        height      = @Height,
        bodyWeight  = @Weight
    WHERE PersonId = @PlayerId;
END
GO

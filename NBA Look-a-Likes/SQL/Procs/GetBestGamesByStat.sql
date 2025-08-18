SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.GetBestGamesByStat
    @PlayerId int,
    @Stat varchar(10),
    @Top int = 5
AS
BEGIN
    SET NOCOUNT ON;

    WITH LatestTeams AS (
        SELECT t.*
        FROM Team t
        JOIN (
            SELECT teamId, MAX(seasonActiveTill) AS MaxSeason
            FROM Team
            GROUP BY teamId
        ) latest 
            ON t.teamId = latest.teamId 
           AND t.seasonActiveTill = latest.MaxSeason
    )
    SELECT TOP (@Top)
        GS.GameDate,
        CASE
            WHEN @Stat = 'PPG' THEN GS.Points
            WHEN @Stat = 'RPG' THEN GS.ReboundsTotal
            WHEN @Stat = 'APG' THEN GS.Assists
            WHEN @Stat = 'STL' THEN GS.Steals
            WHEN @Stat = 'BLK' THEN GS.Blocks
            WHEN @Stat = '3PM' THEN GS.ThreePointersMade
            WHEN @Stat = 'FTM' THEN GS.FreeThrowsMade
        END AS StatValue,
        GS.Home,
        GS.GameType,
        G.HomeScore,
        G.AwayScore,
        Home.teamId AS HomeId,
        Away.teamId AS AwayId
    FROM GameStats GS
    JOIN Game G 
        ON G.GameId = GS.GameId
    JOIN LatestTeams Home 
        ON G.HomeTeamId = Home.TeamId
    JOIN LatestTeams Away 
        ON G.AwayTeamId = Away.TeamId
    WHERE GS.PersonId = @PlayerId
    ORDER BY
        CASE
            WHEN @Stat = 'PPG' THEN GS.Points
            WHEN @Stat = 'RPG' THEN GS.ReboundsTotal
            WHEN @Stat = 'APG' THEN GS.Assists
            WHEN @Stat = 'STL' THEN GS.Steals
            WHEN @Stat = 'BLK' THEN GS.Blocks
            WHEN @Stat = '3PM' THEN GS.ThreePointersMade
            WHEN @Stat = 'FTM' THEN GS.FreeThrowsMade
        END DESC;
END
GO

INSERT INTO dbo.Game (
    gameId,
    gameDate,
    hometeamid,
    hometeamCity,
    hometeamName,
    awayteamid,
    awayteamCity,
    awayteamName,
    homeScore,
    awayScore,
    winner,
    gameType,
    attendance
   
)
OUTPUT INSERTED.gameId
VALUES (
    @gameId,
    @gameDate,
    @homeTeamId,
    @homeCity,
    @homeName,
    @awayTeamId,
    @awayCity,
    @awayName,
    @homeScore,
    @awayScore,
    @winner,
    @gameType,
    @attendance
);
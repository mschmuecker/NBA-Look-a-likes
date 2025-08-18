using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NBA_App.DTO;
using NBA_App.Enums;
using NBA_App.Extensions;
using NBA_App.Model;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NBA_App.Data
{
    public class GameDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public GameDataAccess(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default");
            _configuration = config;
        }
        public async Task<List<StatLeader>> GetStatLeadersAsync(StatCategory stat)
        {
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetStatLeaders", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Stat", StatCategoryExtensions.ToSqlValue(stat));

                List<StatLeader> leaders = new();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    StatLeader statLeader = new StatLeader
                    {
                        Player = PlayerDataAccess.GetPlayerData(reader),
                        Stat = SQLRead.GetSafeDouble(reader, "statvalue")

                    };
                    leaders.Add(statLeader);
                }
                return leaders;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Type: " + ex.GetType().FullName);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
            }
            return null;
        }
        public async Task<List<GameSummary>> GetGamesOnDate(DateTime date)
        {
            List<GameSummary> games = new();

            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetGameByDate", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GameDate", date);
                
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    GameSummary game = new GameSummary
                    {
                        GameId = SQLRead.GetSafeInt(reader, "gameid"),
                        GameDate = SQLRead.GetSafeDate(reader, "gamedate"),
                        HomeTeamId = SQLRead.GetSafeInt(reader, "hometeamid"),
                        HomeTeamName = SQLRead.GetSafeString(reader, "hometeamname"),
                        HomeTeamCity = SQLRead.GetSafeString(reader, "hometeamcity"),
                        AwayTeamCity = SQLRead.GetSafeString(reader, "awayteamcity"),
                        AwayTeamId = SQLRead.GetSafeInt(reader, "awayteamid"),
                        AwayTeamName = SQLRead.GetSafeString(reader, "awayteamname"),
                        HomeScore = SQLRead.GetSafeTinyInt(reader, "homescore"),
                        AwayScore = SQLRead.GetSafeTinyInt(reader, "awayscore"),
                        Winner = SQLRead.GetSafeInt(reader, "winner"),
                        Attendance = SQLRead.GetSafeDouble(reader, "attendance"),
                        ArenaId = SQLRead.GetSafeInt(reader, "arenaId"),
                        GameType = SQLRead.GetSafeString(reader, "gametype")


                    };
                    games.Add(game);
                }
            }
            catch(Exception ex )
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            return games;
        }
   
        public async Task<int?> InsertGameAsync(GameEditModel model)
        {
            model.GameId = await InsertGameDataAsync(model);
            await InsertGamePlayerDataAsync(model);
            return model.GameId;

        }
        private async Task<int> InsertGameDataAsync(GameEditModel model)
        {
            
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.InsertGame", conn);
            Random rnd = new();
            int gameId = rnd.Next(100000, 999999); // ensure uniqueness manually!

            model.GameId = gameId;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GameId", gameId);
            cmd.Parameters.AddWithValue("@GameDate", model.GameDate);
            cmd.Parameters.AddWithValue("@HomeTeamId", model.HomeTeam.TeamID);
            cmd.Parameters.AddWithValue("@HomeCity", model.HomeTeam.TeamCity);
            cmd.Parameters.AddWithValue("@HomeName", model.HomeTeam.TeamName);
            cmd.Parameters.AddWithValue("@AwayTeamId", model.AwayTeam.TeamID);
            cmd.Parameters.AddWithValue("@AwayName", model.AwayTeam.TeamName);
            cmd.Parameters.AddWithValue("@AwayCity", model.AwayTeam.TeamCity);
            cmd.Parameters.AddWithValue("@HomeScore", model.HomeScore);
            cmd.Parameters.AddWithValue("@AwayScore", model.AwayScore);
            cmd.Parameters.AddWithValue("@Winner", model.HomeScore > model.AwayScore ? model.HomeTeam.TeamID
                : model.AwayTeam.TeamID);
            cmd.Parameters.AddWithValue("@GameType", model.GameType);
            cmd.Parameters.AddWithValue("@Attendance", model.Attendance);

            return (int)await cmd.ExecuteScalarAsync();
           
        }
        private async Task InsertGamePlayerDataAsync(GameEditModel model)
        {
            
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            foreach (var row in model.Players)
            {
                using SqlCommand cmd = new("dbo.InsertGameStats", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GameId", model.GameId);
                cmd.Parameters.AddWithValue("@PersonId", row.Player.PlayerID);
                cmd.Parameters.AddWithValue("@Minutes", row.Minutes);
                cmd.Parameters.AddWithValue("@Points", row.Points);
                cmd.Parameters.AddWithValue("@Rebounds", row.ReboundsTotal);
                cmd.Parameters.AddWithValue("@Assists", row.Assists);
                cmd.Parameters.AddWithValue("@Home", row.Home);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        
        public async Task UpdateGameAsync(GameEditModel model)
        {
            await UpdateGameStatsAsync(model);
            await UpdatePlayerGameStatsAsync(model);

        }
        private async Task UpdateGameStatsAsync(GameEditModel model)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.UpdateGame", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@GameId", model.GameId);
            cmd.Parameters.AddWithValue("@HomeScore", model.HomeScore);
            cmd.Parameters.AddWithValue("@AwayScore", model.AwayScore);
            cmd.Parameters.AddWithValue("@Winner", model.HomeScore > model.AwayScore ? model.HomeTeam.TeamID
                : model.AwayTeam.TeamID);
            cmd.Parameters.AddWithValue("@GameType", model.GameType);
            cmd.Parameters.AddWithValue("@Attendance", model.Attendance);

            await cmd.ExecuteNonQueryAsync();

        }
        private async Task UpdatePlayerGameStatsAsync(GameEditModel model)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            foreach (var row in model.Players)
            {
                using SqlCommand cmd = new("dbo.UpdateGameStats", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GameId", model.GameId);
                cmd.Parameters.AddWithValue("@PersonId", row.Player.PlayerID);
                cmd.Parameters.AddWithValue("@Minutes", row.Minutes);
                cmd.Parameters.AddWithValue("@Points", row.Points);
                cmd.Parameters.AddWithValue("@Rebounds", row.ReboundsTotal);
                cmd.Parameters.AddWithValue("@Assists", row.Assists);

                await cmd.ExecuteNonQueryAsync();
            }

        }
        public async Task<GameEditModel> LoadGameForEditAsync(int gameId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string sql = File.ReadAllText("SQL/LoadGame.sql");
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@gameId", gameId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                return new GameEditModel
                {
                    GameId = gameId,
                    GameDate = SQLRead.GetSafeDate(reader, "gameDate"),
                    HomeTeamId = SQLRead.GetSafeInt(reader, "hometeamid"),
                    AwayTeamId = SQLRead.GetSafeInt(reader, "awayteamid"),
                    HomeScore = SQLRead.GetSafeTinyInt(reader, "homescore"),
                    AwayScore = SQLRead.GetSafeTinyInt(reader, "awayscore"),
                    Winner = SQLRead.GetSafeInt(reader, "winner"),
                    GameType = SQLRead.GetSafeString(reader, "gameType"),
                    Attendance = SQLRead.GetSafeDouble(reader, "attendance"),
                    ArenaId = SQLRead.GetSafeInt(reader,"arenaId")
                };


            }

            return new();
        }
        public async Task<List<PlayerGameRow>> LoadPlayerGameInfoAsync(int gameId)
        {
            List<PlayerGameRow> rows = new List<PlayerGameRow>();
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetGameStatsByGameId", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GameId", gameId);

                using var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    PlayerDataAccess db = new(_configuration);
                    PlayerGameRow row = new PlayerGameRow
                    {
                        Win = SQLRead.GetSafeBoolean(reader, "win"),
                        Home = SQLRead.GetSafeBoolean(reader, "home"),
                        Minutes = SQLRead.GetSafeDouble(reader, "numMinutes"),
                        Points = SQLRead.GetSafeDouble(reader, "points"),
                        Assists = SQLRead.GetSafeDouble(reader, "assists"),
                        Blocks = SQLRead.GetSafeDouble(reader, "blocks"),
                        Steals = SQLRead.GetSafeDouble(reader, "steals"),
                        FGA = SQLRead.GetSafeDouble(reader, "fieldGoalsAttempted"),
                        FGM = SQLRead.GetSafeDouble(reader, "fieldGoalsMade"),
                        TPA = SQLRead.GetSafeDouble(reader, "threepointersattempted"),
                        TPM = SQLRead.GetSafeDouble(reader, "threepointersmade"),
                        FTA = SQLRead.GetSafeDouble(reader, "freethrowsattempted"),
                        FTM = SQLRead.GetSafeDouble(reader, "freethrowsmade"),
                        ReboundsTotal = SQLRead.GetSafeDouble(reader, "reboundstotal"),
                        Turnovers = SQLRead.GetSafeDouble(reader, "turnovers"),
                        Player = await db.GetPlayerByIdAsync(SQLRead.GetSafeInt(reader,"personid"))
                        
                    };
                    rows.Add(row);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rows;
        }
    }
}

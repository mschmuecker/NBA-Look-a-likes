namespace NBA_App.Data
{
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using NBA_App.DTO;
    using NBA_App.Enums;
    using NBA_App.Model;
    using System.Data;
    using System.Text;
    using System.Xml.Linq;

    public class PlayerDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private TeamDataAccess teamDataAccess;

        public PlayerDataAccess(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default");
            _configuration = config;
             teamDataAccess = new(_configuration);
        }
        /// <summary>
        /// This will load all players from database into memory
        /// </summary>
        /// <returns></returns>
        public async Task<List<Player?>> GetAll()
        {
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                string sql = File.ReadAllText("SQL/AllPlayers.sql");
                using SqlCommand cmd = new(sql, conn);
                List<Player> players = new();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    players.Add(GetPlayerData(reader));

                }
                return players;
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
        public async Task UpdatePlayerAsync(Player player)
        {
            using var conn = new SqlConnection(_connectionString);
            string sql = File.ReadAllText("SQL/UpdatePlayer.sql");
            using SqlCommand cmd = new("dbo.UpdatePlayer", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PlayerId", player.PlayerID);
            cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
            cmd.Parameters.AddWithValue("@LastName", player.LastName);
            cmd.Parameters.AddWithValue("@College", player.LastCollege);
            cmd.Parameters.AddWithValue("@Height", player.Height);
            cmd.Parameters.AddWithValue("@Weight", player.Weight);
            cmd.Parameters.AddWithValue("@IsGuard", player.Position == "Guard" ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsForward", player.Position == "Forward" ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsCenter", player.Position == "Center" ? 1 : 0);
            cmd.Parameters.AddWithValue("@DraftYear", player.DraftYear);
            cmd.Parameters.AddWithValue("@DraftRound", player.DraftRound);
            cmd.Parameters.AddWithValue("@DraftNumber", player.DraftNumber);


            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// This gets player key demographic information given playerId
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<Player?> GetPlayerByIdAsync(int playerId)
        {
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetPlayerById", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlayerId", playerId);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return GetPlayerData(reader);
                }
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

        public async Task<College?> GetCollegeByIdAsync(int collegeId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT * FROM College WHERE CollegeID = @collegeId";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@collegeId", collegeId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new College
                {
                    CollegeID = reader.GetInt32(0),
                    CollegeName = reader.GetString(1),
                    CollegeCity = reader.GetString(2),
                    CollegeState = reader.GetString(3),
                    CollegeCountry = reader.GetString(4),
                    Mascot = reader.GetString(5)
                };
            }

            return null;
        }
        /// <summary>
        /// This will return the career stat summary for key statistics given playerID
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<StatSummary?> GetCareerStatsAsync(int playerId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();


            using SqlCommand cmd = new("dbo.GetPlayerAverages", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PlayerId", playerId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new StatSummary
                {
                    PPG = SQLRead.GetSafeDouble(reader, "PPG"),
                    RPG = SQLRead.GetSafeDouble(reader, "RPG"),
                    APG = SQLRead.GetSafeDouble(reader, "APG"),
                    FGPercentage = SQLRead.GetSafeDouble(reader, "FGPercentage"),
                    ThreePPercentage = SQLRead.GetSafeDouble(reader, "ThreePPercentage"),
                    Turnovers = SQLRead.GetSafeDouble(reader, "Turnovers"),
                    FTPercentage = SQLRead.GetSafeDouble(reader, "FT"),
                    SPG = SQLRead.GetSafeDouble(reader,"SPG"),
                    BPG = SQLRead.GetSafeDouble(reader,"BPG")

                };

            }
            return null;
        }
        /// <summary>
        /// Get all season stats for a player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<StatSummary?>> GetSeasonStatsAsync(int playerId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetPlayerSeasonStats", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@playerId", playerId);
            List <StatSummary> seasonStats= new();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                seasonStats.Add(new StatSummary
                {
                    Season = SQLRead.GetSafeInt(reader, "SeasonYear"),
                    PPG = SQLRead.GetSafeDouble(reader, "PPG"),
                    RPG = SQLRead.GetSafeDouble(reader, "RPG"),
                    APG = SQLRead.GetSafeDouble(reader, "APG"),
                    FGPercentage = SQLRead.GetSafeDouble(reader, "FGPercentage"),
                    ThreePPercentage = SQLRead.GetSafeDouble(reader, "ThreePPercentage"),
                    Turnovers = SQLRead.GetSafeDouble(reader, "Turnovers"),
                    FTPercentage = SQLRead.GetSafeDouble(reader, "FT"),
                    SPG = SQLRead.GetSafeDouble(reader, "SPG"),
                    BPG = SQLRead.GetSafeDouble(reader, "BPG")

                });

            }
            return seasonStats;
        }
        /// <summary>
        /// This will get similar players based on PPG, RPG, Assists
        /// By default it will return the list of 5 most similar players
        /// This uses a euler method for calculating similarity
        /// </summary>
        /// <param name="playerId">Player to search</param>
        /// <param name="top"></param>
        /// <returns></returns>
        public async Task<List<Player>> GetSimilarPlayersAsync(int playerId, int top = 5)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetSimilarPlayersByStats", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //params for query
            cmd.Parameters.AddWithValue("@PlayerId", playerId);
            cmd.Parameters.AddWithValue("@Top", top);

            List<Player> players = new List<Player>();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Player player = new();
                player.PlayerID = SQLRead.GetSafeInt(reader,"personId");
                player.FirstName = SQLRead.GetSafeString(reader, "firstName");
                player.LastName = SQLRead.GetSafeString(reader, "lastName");
                player.CareerStatSummary = new StatSummary();
                player.CareerStatSummary.PPG = SQLRead.GetSafeDouble(reader, "PPG");
                player.CareerStatSummary.RPG = SQLRead.GetSafeDouble(reader, "RPG");
                player.CareerStatSummary.APG = SQLRead.GetSafeDouble(reader, "APG");
                players.Add(player);
            }
            return players;
        }
        public async Task<List<Player>> GetSimilarPlayersByStatsAsync(int playerId, Dictionary<StatCategory, bool> statEnabled
            , Dictionary<StatCategory, double> statWeights)
        {
            List<Player> players = new();
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetSimilarPlayersByWeightedStats", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                foreach(var category in Enum.GetValues<StatCategory>())
                {
                    string name = category.ToString();

                    bool use = statEnabled.TryGetValue(category, out var enabled) ? enabled : false;
                    double weight = statWeights.TryGetValue(category, out var w) ? w : 1.0;

                    cmd.Parameters.AddWithValue($"@use{name}", use ? 1 : 0);
                    cmd.Parameters.AddWithValue($"@weight{name}", weight);
                }
                //params for query
               
                using var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var PlayerID = SQLRead.GetSafeInt(reader, "personId");
                    Player player = await GetPlayerByIdAsync(PlayerID);
                    player.CareerStatSummary = new StatSummary();
                    player.CareerStatSummary.PPG = SQLRead.GetSafeDouble(reader, "PPG");
                    player.CareerStatSummary.RPG = SQLRead.GetSafeDouble(reader, "RPG");
                    player.CareerStatSummary.APG = SQLRead.GetSafeDouble(reader, "APG");
                    players.Add(player);

                }

               
            }
            catch (Exception ex)
            {

            }
            return players;
        }

        public async Task<List<Player>> FindPlayersByStatsAsync(StatFilter filter)
        {
            var players = new List<Player>();
            var sb = new StringBuilder(@"
        SELECT 
            P.playerID, P.firstName, P.lastName,
            AVG(CAST(GS.fieldGoalsMade + GS.threePointersMade + GS.freeThrowsMade AS FLOAT)) AS PPG,
            AVG(CAST(GS.reboundsTotal AS FLOAT)) AS RPG,
            AVG(CAST(GS.assists AS FLOAT)) AS APG,
            AVG(CAST(GS.fieldGoalsPercentage AS FLOAT)) AS FGPercentage,
            AVG(CAST(GS.threePointersPercentage AS FLOAT)) AS ThreePPercentage,
            AVG(CAST(GS.turnovers AS FLOAT)) AS Turnovers
        FROM Player P
        JOIN GameStats GS ON P.playerID = GS.playerID
        GROUP BY P.playerID, P.firstName, P.lastName
        HAVING 1=1");

            var parameters = new List<SqlParameter>();

            if (filter.MinPPG.HasValue)
            {
                sb.Append(" AND AVG(CAST(GS.fieldGoalsMade + GS.threePointersMade + GS.freeThrowsMade AS FLOAT)) >= @MinPPG");
                parameters.Add(new SqlParameter("@MinPPG", filter.MinPPG));
            }
            if (filter.MaxTurnovers.HasValue)
            {
                sb.Append(" AND AVG(CAST(GS.turnovers AS FLOAT)) <= @MaxTurnovers");
                parameters.Add(new SqlParameter("@MaxTurnovers", filter.MaxTurnovers));
            }
            if (filter.MinThreeP.HasValue)
            {
                sb.Append(" AND AVG(CAST(GS.threePointersPercentage AS FLOAT)) >= @MinThreeP");
                parameters.Add(new SqlParameter("@MinThreeP", filter.MinThreeP));
            }
            if (filter.MinAPG.HasValue)
            {
                sb.Append(" AND AVG(CAST(GS.assists AS FLOAT)) >= @MinAPG");
                parameters.Add(new SqlParameter("@MinAPG", filter.MinAPG));
            }
            if (filter.MinRPG.HasValue)
            {
                sb.Append(" AND AVG(CAST(GS.reboundsTotal AS FLOAT)) >= @MinRPG");
                parameters.Add(new SqlParameter("@MinRPG", filter.MinRPG));
            }
            if (filter.MinFGP.HasValue)
            {
                sb.Append(" AND AVG(CAST(GS.fieldGoalsPercentage AS FLOAT)) >= @MinFGP");
                parameters.Add(new SqlParameter("@MinFGP", filter.MinFGP));
            }

            string sql = sb.ToString();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddRange(parameters.ToArray());

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Player player = GetPlayerData(reader);
                player.CareerStatSummary = new StatSummary
                {
                    PPG = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    RPG = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    APG = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    FGPercentage = reader.IsDBNull(6) ? 0 : reader.GetDouble(6),
                    ThreePPercentage = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                };
                players.Add(player);
            }

            return players;
        }
        /// <summary>
        /// This will get the 5 best games for a player based on the chosen stat
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public async Task<List<BestGameStats>> GetBestGameStatsAsync(int playerId, string stat)
        {
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();


                using SqlCommand cmd = new("dbo.GetBestGamesByStat", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //params for query
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                cmd.Parameters.AddWithValue("@Stat", stat);

                List<BestGameStats> games = new();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    BestGameStats game = new BestGameStats();
                    game.GameDate = SQLRead.GetSafeDate(reader, "GameDate");
                    game.StatValue = SQLRead.GetSafeDouble(reader, "StatValue");
                    game.WasHome = SQLRead.GetSafeBoolean(reader, "home");
                    game.GameType = SQLRead.GetSafeString(reader, "gametype");
                    game.AwayPoints = SQLRead.GetSafeTinyInt(reader, "awayScore");
                    game.HomePoints = SQLRead.GetSafeTinyInt(reader, "homeScore");
                    int homeTeam = SQLRead.GetSafeInt(reader, "homeId");
                    int awayTeam = SQLRead.GetSafeInt(reader, "awayId");

                    
                    //now need to look up home Team and away team from the team service
                    
                    game.Home = await teamDataAccess.GetTeamInfoAsync(homeTeam);
                    game.Away = await teamDataAccess.GetTeamInfoAsync(awayTeam);

                    games.Add(game);

                }
                return games;
            }
            catch(Exception ex)
            { }
            return null;
        }
        public async Task<List<PlayedFor>> GetTeamsPlayedForAsync(Player player)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetTeamsPlayedForByPlayer", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //params for query
            cmd.Parameters.AddWithValue("@PlayerId", player.PlayerID);

            List<PlayedFor> teams = new();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                PlayedFor playedFor = new(); 

                int teamId = SQLRead.GetSafeInt(reader, "teamid");
                playedFor.Team = await teamDataAccess.GetTeamInfoAsync(teamId);
                playedFor.Player = player;
                playedFor.StartSeason = SQLRead.GetSafeShort(reader, "startseason");
                playedFor.EndSeason = SQLRead.GetSafeShort(reader, "endseason");
                teams.Add(playedFor);
                
            }
            return teams;

        }
        /// <summary>
        /// This will consistently get the core columns when reading from dbo.Players
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Player GetPlayerData(SqlDataReader reader)
        {
            bool isGuard = SQLRead.GetSafeBoolean(reader, "guard");
            bool isForward = SQLRead.GetSafeBoolean(reader, "forward");
            bool isCenter = SQLRead.GetSafeBoolean(reader, "center");

            return new Player
            {
                PlayerID = SQLRead.GetSafeInt(reader, "personId"),
                FirstName = SQLRead.GetSafeString(reader, "firstName"),
                LastName = SQLRead.GetSafeString(reader, "lastName"),
                BirthDate = SQLRead.GetSafeDate(reader, "birthdate"),
                Height = SQLRead.GetSafeDouble(reader, "height"),
                Weight = SQLRead.GetSafeDouble(reader, "bodyWeight"),
                DraftYear = SQLRead.GetSafeDouble(reader, "draftyear"),
                DraftRound = SQLRead.GetSafeDouble(reader, "draftround"),
                DraftNumber = SQLRead.GetSafeDouble(reader, "draftNumber"),
                LastCollege = SQLRead.GetSafeString(reader, "lastAttended"),
                Position = isGuard ? "Guard" : isForward ? "Foward" : isCenter ? "Center" : "Unknown"
            };

        }
        public async Task<List<Player>> GetPlayersByTeamAsync(int teamId)
        {
            return new List<Player>();
        }
    }
}
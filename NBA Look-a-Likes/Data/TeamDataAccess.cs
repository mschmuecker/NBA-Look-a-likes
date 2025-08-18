using Microsoft.Data.SqlClient;
using NBA_App.Components.Pages;
using NBA_App.DTO;
using NBA_App.Model;

namespace NBA_App.Data
{
    public class TeamDataAccess
    {
        private readonly string _connectionString;

        public TeamDataAccess(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default");
        }
        /// <summary>
        /// This will load all Teams and organize them by most recent team using GetTeamInfoAsync API
        /// </summary>
        /// <returns></returns>
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            List<Team> teams = new();
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string sql = File.ReadAllText("SQL/GetAllTeams.sql");
            using SqlCommand cmd = new(sql, conn);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                teams.Add(await GetTeamInfoAsync(SQLRead.GetSafeInt(reader, "teamId")));
            }
            //first get all unique Teams then load the full history for each
            return teams;
        }

        /// <summary>
        /// Will pull the team information from the db based on passed in teamId
        /// </summary>
        /// <param name="teamId"></param>
        public async Task<Team> GetTeamInfoAsync(int teamId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetTeamById", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamId", teamId);
            bool first = true;
            Team team = new Team { TeamID = teamId };

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var historyRecord = GetTeamHistoryData(reader);
                //we are sorting by recent team so save the current team since most commonly used
                if (first)
                {
                    team.TeamName = historyRecord.TeamName;
                    team.TeamAbbrev = historyRecord.TeamAbbrev;
                    team.League = historyRecord.League;
                    team.TeamCity = historyRecord.TeamCity;
                    first = false;
                }
                team.History.Add(historyRecord);
            }
            return team;

        }
        /// <summary>
        /// Gets teams as of a specific season
        /// </summary>
        /// <param name="season"></param>
        /// <returns></returns>
        public async Task<List<Team>> GetTeamsOnDateAsync(short season)
        {
            List<Team> teams = new List<Team>();
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetTeamsBySeason", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Season", season);


                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Team team = new Team
                    {
                        TeamID = SQLRead.GetSafeInt(reader, "teamid"),
                        TeamName = SQLRead.GetSafeString(reader, "TeamName"),
                        TeamAbbrev = SQLRead.GetSafeString(reader, "TeamAbbrev"),
                        League = SQLRead.GetSafeString(reader, "League"),
                        TeamCity = SQLRead.GetSafeString(reader, "teamCity")
                    };
                    teams.Add(team);
                }
                
            }
            catch (Exception ex) { }
            return teams;
        }
        /// <summary>
        /// Grabs the players on the team based on the season
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        public async Task<List<Player>> GetPlayersOnTeam(int teamId,  short season)
        {
            List<Player> players = new List<Player>();
            try
            {
                using SqlConnection conn = new(_connectionString);
                await conn.OpenAsync();

                using SqlCommand cmd = new("dbo.GetPlayersByTeamSeason", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamId", teamId);
                cmd.Parameters.AddWithValue("@Season", season);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    players.Add(PlayerDataAccess.GetPlayerData(reader));
                }
            }
            catch (Exception ex) { }
            return players;
                    
        }
        /// <summary>
        /// Gets team all time record
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<TeamSummary> GetTeamRecord(int teamId)
        {

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetTeamRecord", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TeamId", teamId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TeamSummary
                {
                    Wins = SQLRead.GetSafeInt(reader, "wins"),
                    Losses = SQLRead.GetSafeInt(reader, "losses"),
                    GamesPlayed = SQLRead.GetSafeInt(reader, "gamesplayed"),
                };
            }

            return new TeamSummary();
        }
        /// <summary>
        /// Loads all season for given team for high level information
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<List<TeamSummary>> GetTeamRecordEachSeason(int teamId)
        {
            List<TeamSummary> result = new();
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetTeamRecordBySeason", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TeamId", teamId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add( new TeamSummary
                {
                    Wins = SQLRead.GetSafeInt(reader, "wins"),
                    Losses = SQLRead.GetSafeInt(reader, "losses"),
                    GamesPlayed = SQLRead.GetSafeInt(reader, "gamesplayed"),
                    PPG = SQLRead.GetSafeDouble(reader,"PointsPerGame"),
                    PointsAllowed = SQLRead.GetSafeDouble(reader,"PointsAgainstPerGame"),
                    Season = SQLRead.GetSafeInt(reader,"Season")
             
                });
            }

            return result;

        }
        /// <summary>
        /// Saves off the history record for a Team from the database
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private TeamHistory GetTeamHistoryData(SqlDataReader reader)
        {
            return new TeamHistory
            {
                TeamName = SQLRead.GetSafeString(reader, "TeamName"),
                TeamAbbrev = SQLRead.GetSafeString(reader, "TeamAbbrev"),
                SeasonFounded = SQLRead.GetSafeShort(reader, "SeasonFounded"),
                SeasonActiveTill = SQLRead.GetSafeShort(reader, "SeasonActiveTill"),
                League = SQLRead.GetSafeString(reader, "League"),
                TeamCity = SQLRead.GetSafeString(reader, "teamCity")

            };
        }
    }
}

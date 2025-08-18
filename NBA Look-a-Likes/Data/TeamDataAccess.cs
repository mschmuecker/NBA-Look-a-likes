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

            string sql = File.ReadAllText("SQL/GetTeam.sql");
            using SqlCommand cmd = new(sql, conn);
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
        public async Task<TeamRecordSummary> GetTeamRecord(int teamId)
        {

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new("dbo.GetTeamRecord", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TeamId", teamId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TeamRecordSummary
                {
                    Wins = SQLRead.GetSafeInt(reader, "wins"),
                    Losses = SQLRead.GetSafeInt(reader, "losses"),
                    GamesPlayed = SQLRead.GetSafeInt(reader, "gamesplayed"),
                };
            }

            return new TeamRecordSummary();
        }
        /// <summary>
        /// Gets the list of all the active teams
        /// </summary>
        /// <param name="ActiveOnly">If a team is no longer active don't return</param>
        /// <param name="League">limit to only this type of league</param>
        /// <returns></returns>
        private async Task<List<Team>> GetTeamInfoAsync(bool ActiveOnly,string League)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            //TO DO
            string sql = File.ReadAllText("SQL/GetTeam.sql");
            using SqlCommand cmd = new(sql, conn);
            //
            using var reader = await cmd.ExecuteReaderAsync();
            List<Team> result = new List<Team>();
            while (await reader.ReadAsync())
            {
                //
            }
            return result;

        }
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

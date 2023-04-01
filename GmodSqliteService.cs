using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text.Json;
using System.Text.Json.Serialization;
using TTT2StatsApi.Models;

namespace TTT2StatsApi
{
    public interface IGmodSqliteService
    {
        IEnumerable<CombatLogRow> GetCombatLog(int rowLimit = 30);
        IEnumerable<MapCountRow> GetMapCounts();
        IEnumerable<RoundRow> GetRounds(int rowLimit = 100);
        RoundRow? GetRoundById(int id);
        IEnumerable<RoundRow> GetRoundsByMap(string map, int rowLimit = 100);
        IEnumerable<PlayerRoundInfoRow> GetPlayerRoundInfos(int roundId);
        IEnumerable<PlayerDeathRow> GetPlayerKillsDeathsByRoundIdAndSteamId(int id, string steamId);
    }

    public class GmodSqliteService : IGmodSqliteService
    {
        private readonly ILogger<GmodSqliteService> _logger;
        private readonly string _connectionString;
        private readonly string gmodSvPath;
        private readonly string PlayerRoundInfoQueryText;
        private readonly string RoundPlayerDeathQueryText;
        private readonly string CombatLogQueryText;
        public GmodSqliteService(ILogger<GmodSqliteService> logger, IConfiguration configuration)
        {
            _logger = logger;
            gmodSvPath = configuration.GetValue<string>("SV_Path");
            if (string.IsNullOrWhiteSpace(gmodSvPath))
            {
                _logger.LogCritical("The path to the Garry's Mod sv.db file was not set!");
            }
            _connectionString = $"Data Source={gmodSvPath};Compress=True;";
            // Read ~\sql\PlayerRoundInfoQuery.sql file into PlayerRoundInfoQueryText
            PlayerRoundInfoQueryText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "sql", "PlayerRoundInfoQuery.sql"));
            RoundPlayerDeathQueryText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "sql", "RoundPlayerDeathsQuery.sql"));
            CombatLogQueryText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "sql", "CombatLogQuery.sql"));
        }
        public IEnumerable<CombatLogRow> GetCombatLog(int rowLimit = 30)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = CombatLogQueryText;
            command.Parameters.AddWithValue("rowLimit", rowLimit);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new CombatLogRow
                {
                    Id = reader.GetInt64(0),
                    DamageTime = reader.GetInt64(1),
                    AttackerName = reader.GetString(2),
                    AttackerSteamId = reader.GetString(3),
                    VictimName = reader.GetString(4),
                    VictimSteamId = reader.GetString(5),
                    DamageDealt = reader.GetDouble(6),
                    HealthRemaining = reader.GetDouble(7),
                    Weapon = reader.GetString(8)
                };
            }
        }
        public IEnumerable<RoundRow> GetRounds(int rowLimit = 100)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM ttt2stats_rounds ORDER BY id DESC LIMIT {rowLimit};";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new RoundRow
                {
                    Id = reader.GetInt64(0),
                    Map = reader.GetString(1),
                    StartTime = reader.GetInt64(2),
                    EndTime = reader.IsDBNull(3) ? null : reader.GetInt64(3),
                    EndedNormally = reader.GetInt16(4) == 1 ? true : false,
                    Result = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
        }
        public RoundRow? GetRoundById(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ttt2stats_rounds WHERE id = @id LIMIT 1;";
            command.Parameters.AddWithValue("id", id);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                return new RoundRow
                {
                    Id = reader.GetInt64(0),
                    Map = reader.GetString(1),
                    StartTime = reader.GetInt64(2),
                    EndTime = reader.IsDBNull(3) ? null : reader.GetInt64(3),
                    EndedNormally = reader.GetInt16(4) == 1 ? true : false,
                    Result = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
            return null;
        }
        public IEnumerable<RoundRow> GetRoundsByMap(string map, int rowLimit = 100)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM ttt2stats_rounds WHERE map=@map ORDER BY id DESC LIMIT {rowLimit};";
            command.Parameters.AddWithValue("@map", map);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new RoundRow
                {
                    Id = reader.GetInt64(0),
                    Map = reader.GetString(1),
                    StartTime = reader.GetInt64(2),
                    EndTime = reader.IsDBNull(3) ? null : reader.GetInt64(3),
                    EndedNormally = reader.GetInt16(4) == 1 ? true : false,
                    Result = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
        }
        public IEnumerable<MapCountRow> GetMapCounts()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM v_GetMapPlayCount;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new MapCountRow
                {
                    Map = reader.GetString(0),
                    RoundsCompletedCount = reader.GetInt64(1),
                    RoundsIncompleteCount = reader.GetInt64(2),
                };
            }
        }
        public IEnumerable<PlayerRoundInfoRow> GetPlayerRoundInfos(int roundId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = PlayerRoundInfoQueryText;
            command.Parameters.AddWithValue("@roundId", roundId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new PlayerRoundInfoRow
                {
                    RoundId = reader.GetInt64(0),
                    SteamId = Convert.ToString(reader.GetValue(1)),
                    FriendlyName = reader.GetString(2),
                    PlayerRole = reader.GetString(3),
                    StartingKarma = reader.GetDouble(4),
                    EndingKarma = reader.GetDouble(5),
                    Kills = reader.GetInt64(6),
                    Deaths = reader.GetInt64(7),
                };
            }
        }

        public IEnumerable<PlayerDeathRow> GetPlayerKillsDeathsByRoundIdAndSteamId(int id, string steamId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = RoundPlayerDeathQueryText;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@steamId", steamId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string? deathFlagsJson = reader.IsDBNull(8) ? null : reader.GetString(8);
                Dictionary<string, bool>? deathFlags = null;
                if (!string.IsNullOrWhiteSpace(deathFlagsJson))
                {
                    deathFlags = JsonSerializer.Deserialize<Dictionary<string, bool>>(deathFlagsJson);
                }
                var pdr = new PlayerDeathRow
                {
                    id = reader.GetInt64(0),
                    RoundId = reader.GetInt64(1),
                    VictimSteamId = reader.GetString(2),
                    VictimName = reader.GetString(3),
                    KillerSteamId = reader.GetString(4),
                    KillerName = reader.GetString(5),
                    DeathTime = reader.GetInt64(6),
                    DeathCause = reader.GetString(7),
                    DeathFlags = deathFlags
                };
                yield return pdr;
            }
        }
    }
}

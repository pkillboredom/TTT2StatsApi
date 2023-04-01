namespace TTT2StatsApi.Models
{
    public class PlayerDeathRow
    {
        public long id { get; set; }
        public long RoundId { get; set; }
        public string VictimSteamId { get; set; }
        public string VictimName { get; set; }
        public string KillerSteamId { get; set; }
        public string KillerName { get; set; }
        public long DeathTime { get; set; }
        public string DeathCause { get; set; }
        public Dictionary<string, bool>? DeathFlags { get; set; }
    }
}

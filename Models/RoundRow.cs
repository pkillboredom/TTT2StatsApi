namespace TTT2StatsApi.Models
{
    public class RoundRow
    {
        public long Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Map { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public long StartTime { get; set; }
        public long? EndTime { get; set; }
        public bool EndedNormally { get; set; }
        public string? Result { get; set; }
    }
}

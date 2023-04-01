namespace TTT2StatsApi.Models
{
    public class MapCountRow
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Map { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public long RoundsCompletedCount { get; set; }
        public long RoundsIncompleteCount { get; set; }
    }
}

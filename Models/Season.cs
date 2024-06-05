namespace NhlStats.Models
{
    public class Season
    {
        public int SeasonId { get; set; }
        public string EndDate { get; set; }

        public string FormattedSeasonId => $"{SeasonId / 10000}-{SeasonId % 10000}";
    }
}

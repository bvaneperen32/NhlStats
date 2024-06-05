namespace NhlStats.Models
{
    public class Team
    {
        public string TeamName { get; set; }
        public int Wins { get; set; }
        public string TeamLogo { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public int Losses { get; set; }
        public int OtLosses { get; set; }
        public double WinPctg { get; set; }
        public int GoalDifferential { get; set; }
        public int SeasonId { get; set; }
        public int Points { get; set; }

        public string RoundedWinPctg => WinPctg.ToString("F3");

        public string FormattedSeasonId => $"{SeasonId / 10000}-{SeasonId % 10000}"; 
    }
}

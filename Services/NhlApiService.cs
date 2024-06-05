using NhlStats.Models;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json.Linq; 
namespace NhlStats.Services
{
    public class NhlApiService
    {
        private readonly HttpClient _httpClient;

        public NhlApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Season>> GetSeasonsAsync()
        { 
            var response = await _httpClient.GetStringAsync("https://api-web.nhle.com/v1/standings-season");
            JObject json = JObject.Parse(response);
            List<Season> seasons = new List<Season>();

            foreach (var item in json["seasons"])
            {
                seasons.Add(new Season
                {
                    SeasonId = (int) item["id"],
                    EndDate = (string)item["standingsEnd"]
                });
            }

            return seasons;
        }

        public async Task<List<Team>> GetTeamsAsync(string endDate)
        {
            var response = await _httpClient.GetStringAsync($"https://api-web.nhle.com/v1/standings/{endDate}");
            JObject json = JObject.Parse(response);
            List<Team> teams = new List<Team>();

            foreach (var item in json["standings"])
            {
                teams.Add(new Team
                {
                    TeamName = item["teamName"]?["default"]?.ToString() ?? "Unknown",
                    Wins = item["wins"] != null ? (int)item["wins"] : 0,
                    TeamLogo = item["teamLogo"]?.ToString() ?? "",
                    Conference = item["conferenceName"]?.ToString() ?? "Unknown",
                    Division = item["divisionName"]?.ToString() ?? "Unknown",
                    Losses = item["losses"] != null ? (int)item["losses"] : 0,
                    OtLosses = item["otLosses"] != null ? (int)item["otLosses"] : 0,
                    WinPctg = item["winPctg"] != null ? (double)item["winPctg"] : 0.0,
                    GoalDifferential = item["goalDifferential"] != null ? (int)item["goalDifferential"] : 0,
                    SeasonId = item["seasonId"] != null ? (int)item["seasonId"] : 0,
                    Points = item["points"] != null ? (int)item["points"] : 0

                });
            }
            return teams;
        }

        public async Task<Team> getTeamAsync(string endDate, string teamName)
        {
            var teams = await GetTeamsAsync(endDate);
            return teams.FirstOrDefault(t => t.TeamName == teamName); 
        }
    }
}

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

        public async Task<List<Team>> GetTeamsAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api-web.nhle.com/v1/standings/2024-04-18");
            JObject json = JObject.Parse(response);
            List<Team> teams = new List<Team>();

            foreach (var item in json["standings"])
            {
                teams.Add(new Team
                {
                    TeamName = item["teamName"]["default"].ToString(),
                    Wins = (int)item["wins"],
                    TeamLogo = item["teamLogo"].ToString(),
                    Conference = item["conferenceName"].ToString(),
                    Division = item["divisionName"].ToString(),
                    Losses = (int)item["losses"],
                    OtLosses = (int)item["otLosses"],
                    WinPctg = (double)item["winPctg"],
                    GoalDifferential = (int)item["goalDifferential"],
                    SeasonId = (int)item["seasonId"],
                    Points = (int)item["points"]

                });
            }
            return teams;
        }

        public async Task<Team> getTeamAsync(string teamName)
        {
            var teams = await GetTeamsAsync();
            return teams.FirstOrDefault(t => t.TeamName == teamName); 
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using NhlStats.Services;
using NhlStats.Models;
using System.Xml.XPath;


namespace NhlStats.Controllers
{
    public class TeamsController : Controller
    {
        private readonly NhlApiService _nhlApiService;

        public TeamsController(NhlApiService nhlApiService)
        {
            _nhlApiService = nhlApiService;
        }

        public async Task<IActionResult> Index(string sortOrder, int? selectedSeasonId)
        {
            List<Season> seasons = await _nhlApiService.GetSeasonsAsync();
            ViewData["Seasons"] = seasons;

            selectedSeasonId ??= seasons.Max(s => s.SeasonId); 

            Season selectedSeason = seasons.FirstOrDefault(s => s.SeasonId == selectedSeasonId);
            List<Team> teams = await _nhlApiService.GetTeamsAsync(selectedSeason.EndDate);

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "wins_desc" : sortOrder;

            ViewData["SortOrder"] = sortOrder;
            ViewData["SelectedSeasonId"] = selectedSeasonId; 

            teams = sortOrder switch
            {
                "wins_asc" => teams.OrderBy(t => t.Wins).ToList(),
                "wins_desc" => teams.OrderByDescending(t => t.Wins).ToList(),
                "points_asc" => teams.OrderBy(t => t.Points).ToList(),
                "points_desc" => teams.OrderByDescending(t => t.Points).ToList(),
                _ => teams.OrderByDescending(t => t.Wins).ToList(),
            };

            return View(teams);
        }

        public async Task<IActionResult> Details(int seasonId, string teamName)
        {
            List<Season> seasons = await _nhlApiService.GetSeasonsAsync();
            Season selectedseason = seasons.FirstOrDefault(s => s.SeasonId == seasonId);

            if (selectedseason == null)
            {
                return NotFound();
            }

            var team = await _nhlApiService.getTeamAsync(selectedseason.EndDate, teamName);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }
    }
}

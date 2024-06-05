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

        public async Task<IActionResult> Index(string sortOrder)
        {
            List<Team> teams = await _nhlApiService.GetTeamsAsync();

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "wins_desc" : sortOrder;

            ViewData["SortOrder"] = sortOrder;

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

        public async Task<IActionResult> Details(string teamName)
        {
            var team = await _nhlApiService.getTeamAsync(teamName);
            if (team == null)
            {
                return View("Error");
            }

            return View(team);
        }
    }
}

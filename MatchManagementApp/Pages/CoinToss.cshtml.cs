using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages
{
    public class CoinTossModel : PageModel
    {
        private readonly IMatchService _matchService;

        public CoinTossModel(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [BindProperty]
        public MatchDto PendingMatch { get; set; } = default!;

        public IActionResult OnGet()
        {
            if (TempData.TryGetValue("PendingMatch", out var pendingJsonObj) &&
                pendingJsonObj is string pendingJson)
            {
                PendingMatch = JsonSerializer.Deserialize<MatchDto>(pendingJson)
                               ?? new MatchDto();
                TempData["PendingMatch"] = pendingJson;
                return Page();
            }

            return RedirectToPage("/CreateMatch");
        }

        public async Task<IActionResult> OnPostSelectServer(string server)
        {
            if (!TempData.TryGetValue("PendingMatch", out var pendingJsonObj) ||
                 !(pendingJsonObj is string pendingJson))
            {
                return RedirectToPage("/CreateMatch");
            }

            var dto = JsonSerializer.Deserialize<MatchDto>(pendingJson);
            if (dto == null)
            {
                return RedirectToPage("/CreateMatch");
            }

            dto.InitialServer = server;

            var newMatchId = await _matchService.CreateMatchAsync(dto);

            return RedirectToPage("/PlayMatch", new { id = newMatchId });
        }
    }
}

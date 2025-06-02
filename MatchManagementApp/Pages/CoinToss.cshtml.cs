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

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnPostSelectServer(string server)
        {
            await _matchService.UpdateInitialServerAsync(Id, server);
            return RedirectToPage("/PlayMatch", new { id = Id });
        }
    }
}

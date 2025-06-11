using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MatchManagementApp.UI.Pages
{
    public class CoinTossModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public MatchDto PendingMatch { get; set; } = default!;

        public CoinTossModel(IMatchService matchService, IUserService userService)
        {
            _matchService = matchService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetProfileImageAsync();

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

        private async Task SetProfileImageAsync()
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId != null)
            {
                var user = await _userService.GetUserByIdAsync(userId.Value);
                if (user?.ImageBytes != null && user.ImageBytes.Length > 0)
                {
                    var base64 = Convert.ToBase64String(user.ImageBytes);
                    ViewData["ProfileImageBase64"] = $"data:image/png;base64,{base64}";
                    return;
                }
            }
            ViewData["ProfileImageBase64"] = "/images/default-user.png";
        }
    }
}

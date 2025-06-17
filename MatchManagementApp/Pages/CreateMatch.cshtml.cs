using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace MatchManagementApp.UI.Pages
{

    [Authorize]
    public class CreateMatchModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public CreateMatchModel(IMatchService matchService, IUserService userService)
        {
            _matchService = matchService;
            _userService = userService;
        }

        [BindProperty]
        public MatchCreationViewModel Match { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            await SetProfileImageAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SetProfileImageAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }
            var dto = MatchMapper.ToCreateDto(Match, userId.Value);
            TempData["PendingMatch"] = JsonSerializer.Serialize(dto);

            return RedirectToPage("/CoinToss");
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
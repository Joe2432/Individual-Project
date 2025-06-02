using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages
{
    [Authorize]
    public class PlayMatchModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public PlayMatchViewModel? ViewModel { get; private set; }

        public PlayMatchModel(IMatchService matchService, IUserService userService)
        {
            _matchService = matchService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var dto = await _matchService.GetMatchForPlayingAsync(id, userId.Value);
            if (dto == null)
                return NotFound();

            ViewModel = PlayMatchMapper.ToViewModel(dto);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string pointType, bool isUserWinner)
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            await _matchService.RegisterPointAsync(id, userId.Value, pointType, isUserWinner);
            return RedirectToPage(new { id });
        }
    }
}

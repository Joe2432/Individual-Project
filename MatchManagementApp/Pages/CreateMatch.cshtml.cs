using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var dto = MatchMapper.ToCreateDto(Match, userId.Value);
            var matchId = await _matchService.CreateMatchAsync(dto);

            return RedirectToPage("/PlayMatch", new { id = matchId });
        }
    }
}

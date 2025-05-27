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
        private readonly IPointRepository _pointRepo;
        private readonly IMatchRepository _matchRepo;
        private readonly IScorekeepingService _scoreService;

        public PlayMatchModel(
            IMatchService matchService,
            IUserService userService,
            IPointRepository pointRepo,
            IMatchRepository matchRepo,
            IScorekeepingService scoreService)
        {
            _matchService = matchService;
            _userService = userService;
            _pointRepo = pointRepo;
            _matchRepo = matchRepo;
            _scoreService = scoreService;
        }

        public MatchScoreDto? Score { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = await _userService.GetCurrentUserId(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var match = await _matchRepo.GetMatchByIdAsync(id);
            if (match == null)
                return NotFound();

            var points = await _pointRepo.GetPointsByMatchIdAsync(id);
            Score = _scoreService.CalculateScore(match, points);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string pointType, bool isUserWinner)
        {
            var userId = await _userService.GetCurrentUserId(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            await _matchService.RegisterPointAsync(id, userId.Value, pointType, isUserWinner);
            return RedirectToPage(new { id });
        }
    }
}

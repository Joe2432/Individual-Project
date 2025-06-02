using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages
{
    [Authorize]
    public class PlayMatchModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IPointService _pointService;
        private readonly IUserService _userService;
        private readonly IServeStateService _serveStateService;

        public PlayMatchViewModel? MatchViewModel { get; private set; }

        [BindProperty]
        public PointViewModel Point { get; set; } = new();

        public PlayMatchModel(
            IMatchService matchService,
            IPointService pointService,
            IUserService userService,
            IServeStateService serveStateService)
        {
            _matchService = matchService;
            _pointService = pointService;
            _userService = userService;
            _serveStateService = serveStateService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var match = await _matchService.GetMatchForPlayingAsync(id, userId.Value);
            if (match == null)
                return NotFound();

            var points = await _pointService.GetPointsForMatchAsync(id, User);

            MatchViewModel = PlayMatchMapper.ToViewModel(match, points, _serveStateService);

            Point.MatchId = id;
            Point.IsFirstServe = MatchViewModel.IsFirstServe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            // Ensure all Point fields are populated (MatchId, IsUserWinner, etc.)
            // (They should be posted from the form)
            var dto = PointMapper.ToDto(Point);

            await _pointService.RegisterPointAsync(dto, User);

            return RedirectToPage(new { id = Point.MatchId });
        }
    }
}

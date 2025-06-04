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

            var serveState = _serveStateService.GetServeState(match, points);

            MatchViewModel = PlayMatchMapper.ToViewModel(match, points, _serveStateService);

            Point.MatchId = id;
            Point.IsFirstServe = serveState.IsFirstServe;
            Point.CurrentServer = serveState.CurrentServer;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var points = await _pointService.GetPointsForMatchAsync(Point.MatchId, User);
            var match = await _matchService.GetMatchForPlayingAsync(Point.MatchId, userId.Value);

            var serveState = _serveStateService.GetServeState(match, points);

            Point.IsFirstServe = serveState.IsFirstServe;
            Point.CurrentServer = serveState.CurrentServer; // Add this line

            if (Point.PointType == "Fault" && Point.IsFirstServe)
            {
                Point.IsFirstServe = false;
                Point.CurrentServer = serveState.CurrentServer; // Still current server
                MatchViewModel = PlayMatchMapper.ToViewModel(match, points, _serveStateService);
                return Page();
            }
            else if (Point.PointType == "Fault" && !Point.IsFirstServe)
            {
                Point.PointType = "Double Fault";
                Point.IsUserWinner = false;
                Point.IsFirstServe = true;
                Point.CurrentServer = serveState.CurrentServer;
                await _pointService.RegisterPointAsync(PointMapper.ToDto(Point), User);
                return RedirectToPage(new { id = Point.MatchId });
            }
            else
            {
                Point.IsFirstServe = true;
                Point.CurrentServer = serveState.CurrentServer;
                await _pointService.RegisterPointAsync(PointMapper.ToDto(Point), User);
                return RedirectToPage(new { id = Point.MatchId });
            }
        }
    }
}

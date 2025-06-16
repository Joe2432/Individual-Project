using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

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
            await SetProfileImageAsync();

            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToPage("/Account/Login");

            var match = await _matchService.GetMatchForPlayingAsync(id, userId.Value);
            if (match == null) return NotFound();

            var points = await _pointService.GetPointsForMatchAsync(id, User);
            var serveState = _serveStateService.GetServeState(match, points);

            MatchViewModel = PlayMatchMapper.ToViewModel(match, points, _serveStateService);

            Point.MatchId = id;
            Point.IsFirstServe = serveState.IsFirstServe;
            Point.CurrentServer = serveState.CurrentServer;
            Point.NumberOfShots = 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SetProfileImageAsync();

            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToPage("/Account/Login");

            var match = await _matchService.GetMatchForPlayingAsync(Point.MatchId, userId.Value);
            if (match == null) return NotFound();

            var points = await _pointService.GetPointsForMatchAsync(Point.MatchId, User);
            var serveState = _serveStateService.GetServeState(match, points);

            Point.CurrentServer = serveState.CurrentServer;

            if (Request.Form.ContainsKey("IncrementShot"))
            {
                Point.NumberOfShots++;
                ModelState.Remove("Point.NumberOfShots");

                MatchViewModel = PlayMatchMapper.ToViewModel(match, points, _serveStateService);
                return Page();
            }

            if (Point.PointType == "Fault")
            {
                if (Point.IsFirstServe)
                {
                    Point.IsFirstServe = false;
                    ModelState.Remove("Point.IsFirstServe");

                    MatchViewModel = PlayMatchMapper.ToViewModel(match, points, _serveStateService);
                    return Page();
                }
                else
                {
                    Point.PointType = "Double Fault";
                    Point.IsUserWinner = !Point.IsUserWinner;
                    await _pointService.RegisterPointAsync(PointMapper.ToDto(Point), User);
                    return RedirectToPage(new { id = Point.MatchId });
                }
            }

            if (Point.PointType == "Unforced Error" || Point.PointType == "Forced Error")
            {
                Point.IsUserWinner = !Point.IsUserWinner;
            }

            await _pointService.RegisterPointAsync(PointMapper.ToDto(Point), User);
            return RedirectToPage(new { id = Point.MatchId });
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
        public async Task<IActionResult> OnPostUndoAsync()
        {
            await _matchService.UndoLastPointAsync(Point.MatchId);
            return RedirectToPage(new { id = Point.MatchId });
        }
    }
}

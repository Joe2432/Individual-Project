using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchManagementApp.UI.Pages
{
    [Authorize]
    public class MatchHistoryModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;
        private readonly IMatchRepository _matchRepo;
        private readonly IPointRepository _pointRepo;
        private readonly IScorekeepingService _scoreService;

        public MatchHistoryModel(
            IMatchService matchService,
            IUserService userService,
            IMatchRepository matchRepo,
            IPointRepository pointRepo,
            IScorekeepingService scoreService)
        {
            _matchService = matchService;
            _userService = userService;
            _matchRepo = matchRepo;
            _pointRepo = pointRepo;
            _scoreService = scoreService;
        }

        public List<(MatchEntity Match, string ScoreSummary, bool MatchOver)> MatchSummaries { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var userId = await _userService.GetCurrentUserId(User);
            if (userId == null) return;

            var matches = await _matchRepo.GetMatchesByUserIdAsync(userId.Value);

            foreach (var match in matches)
            {
                var points = await _pointRepo.GetPointsByMatchIdAsync(match.Id);
                var score = _scoreService.CalculateScore(match, points);

                var formatted = string.Join(" ", score.SetScores
                    .Where(s => s.Player1Games > 0 || s.Player2Games > 0)
                    .Select(s => s.ToString()));

                MatchSummaries.Add((match, formatted, score.MatchOver));
            }
        }
    }
}

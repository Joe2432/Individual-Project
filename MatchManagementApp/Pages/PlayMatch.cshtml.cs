using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // New properties for the view
        public List<int> DisplaySetIndices { get; private set; } = new();
        public List<string> UserSetGames { get; private set; } = new();
        public List<string> OpponentSetGames { get; private set; } = new();
        public string GameUserDisplay { get; private set; } = "";
        public string GameOpponentDisplay { get; private set; } = "";

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

            PopulateDisplayData();

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

        private void PopulateDisplayData()
        {
            if (Score == null)
                return;

            // Build list of set indices and their scores
            for (int i = 0; i < Score.SetScores.Count; i++)
            {
                var set = Score.SetScores[i];
                bool isFinalSet = i == Score.SetScores.Count - 1;
                bool isEmptyFinalSet = isFinalSet
                    && set.Player1Games == 0
                    && set.Player2Games == 0
                    && Score.MatchOver;

                if (!isEmptyFinalSet)
                {
                    DisplaySetIndices.Add(i);
                    UserSetGames.Add(set.Player1Games.ToString());
                    OpponentSetGames.Add(set.Player2Games.ToString());
                }
            }

            // Split current game score
            var parts = Score.CurrentGameScore.Split('-');
            if (parts.Length == 2)
            {
                GameUserDisplay = parts[0].Trim();
                GameOpponentDisplay = parts[1].Trim();
            }
            else
            {
                GameUserDisplay = GameOpponentDisplay = Score.CurrentGameScore;
            }
        }
    }
}

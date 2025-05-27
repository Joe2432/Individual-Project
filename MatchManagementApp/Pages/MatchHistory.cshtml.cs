using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages
{
    [Authorize]
    public class MatchHistoryModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public List<MatchHistoryViewModel> MatchSummaries { get; private set; } = new();

        public MatchHistoryModel(IMatchService matchService, IUserService userService)
        {
            _matchService = matchService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {

            var userId = await _userService.GetCurrentUserId(User);
            if (userId == null)
                return;

            var dtos = await _matchService.GetMatchHistorySummariesAsync(userId.Value); // Returns List<MatchHistoryDto>
            MatchSummaries = dtos.Select(MatchHistoryMapper.ToViewModel).ToList();
        }
    }
}

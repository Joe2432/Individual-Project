using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages
{
    [Authorize]
    public class MatchHistoryModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public List<MatchHistoryViewModel> MatchSummaries { get; private set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchSurface { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateTo { get; set; }

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
            var dtos = await _matchService.GetMatchHistorySummariesAsync(
            userId.Value,
            name: SearchName,
            type: SearchType,
            surface: SearchSurface,
            date: SearchDate,
            dateFrom: DateFrom,
            dateTo: DateTo
            );


            MatchSummaries = dtos.Select(MatchHistoryMapper.ToViewModel).ToList();
        }
    }
}

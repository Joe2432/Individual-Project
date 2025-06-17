using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchManagementApp.UI.Pages;

[Authorize]
public class MatchHistoryModel : PageModel
{
    private readonly IMatchService _matchService;
    private readonly IUserService _userService;

    public List<MatchHistoryViewModel> MatchSummaries { get; private set; } = new();

    [BindProperty(SupportsGet = true)] public string? SearchName { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchType { get; set; }
    [BindProperty(SupportsGet = true)] public string? SearchSurface { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? SearchDate { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? DateFrom { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? DateTo { get; set; }

    public List<string> AvailableSurfaces { get; set; } = new() { "Hard", "Clay", "Grass" };
    public List<string> AvailableTypes { get; set; } = new() { "Singles", "Doubles" };

    public MatchHistoryModel(IMatchService matchService, IUserService userService)
    {
        _matchService = matchService;
        _userService = userService;
    }

    public async Task OnGetAsync()
    {
        var userId = await _userService.GetCurrentUserIdAsync(User);
        if (userId == null)
        {
            return;
        }
        var user = await _userService.GetUserByIdAsync(userId.Value);
        if (user?.ImageBytes != null && user.ImageBytes.Length > 0)
        {
            var base64 = Convert.ToBase64String(user.ImageBytes);
            ViewData["ProfileImageBase64"] = $"data:image/png;base64,{base64}";
        }
        else
        {
            ViewData["ProfileImageBase64"] = "/images/default-user.png";
        }

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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages
{
    [Authorize]
    public class AnalysisModel : PageModel
    {
        private readonly IAnalysisService _analysisService;
        private readonly IUserService _userService;

        public AnalysisViewModel? ViewModel { get; private set; }

        public AnalysisModel(IAnalysisService analysisService, IUserService userService)
        {
            _analysisService = analysisService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var dto = await _analysisService.GetAnalysisAsync(id, userId.Value);
            ViewModel = AnalysisMapper.ToViewModel(dto);
            if (ViewModel == null)
                return NotFound();

            return Page();
        }
    }
}

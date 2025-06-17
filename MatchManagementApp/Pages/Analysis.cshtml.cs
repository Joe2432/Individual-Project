using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

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
            await SetProfileImageAsync();

            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }
            var dto = await _analysisService.GetAnalysisAsync(id, userId.Value);
            ViewModel = AnalysisMapper.ToViewModel(dto);
            if (ViewModel == null)
            {
                return NotFound();
            }
            return Page();
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
    }
}

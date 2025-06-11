using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace MatchManagementApp.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
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
            }

            ViewData["ProfileImageBase64"] = "/images/default-user.png";
        }
    }
}

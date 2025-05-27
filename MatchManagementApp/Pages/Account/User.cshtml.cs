using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MatchManagementApp.UI.Pages.Account
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly IUserService _userService;

        public UserModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserRegistrationViewModel Profile { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = await _userService.GetCurrentUserId(User);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var dto = await _userService.GetUserViewModelAsync(userId.Value);
            Profile = new UserRegistrationViewModel
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                ConfirmPassword = dto.Password,
                Age = dto.Age,
                Gender = dto.Gender
            };

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Account/Login");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var userId = await _userService.GetCurrentUserId(User);
            if (userId != null)
            {
                await _userService.DeleteUserAsync(userId.Value);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToPage("/Account/Register");
        }
    }
}

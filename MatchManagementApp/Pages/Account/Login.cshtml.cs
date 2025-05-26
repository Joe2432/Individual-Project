using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MatchManagementApp.UI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public LoginModel(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [BindProperty] public string Username { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required.";
                return Page();
            }

            var user = await _userService.GetUserEntityByUsernameAsync(Username);
            if (user == null || !_userService.VerifyPassword(user.PasswordHash, Password))
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            var principal = _authService.GetClaimsPrincipal(user);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true }
            );

            return RedirectToPage("/Index");
        }

    }
}
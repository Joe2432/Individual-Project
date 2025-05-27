using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MatchManagementApp.UI.Mappers;

namespace MatchManagementApp.UI.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public RegisterModel(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [BindProperty]
        public UserRegistrationViewModel UserRegistration { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _userService.GetUserEntityByUsernameAsync(UserRegistration.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Username is already taken.");
                return Page();
            }

            var userDto = UserMapper.ToCreateDto(UserRegistration);
            await _userService.CreateUserAsync(userDto);

            var user = await _userService.GetUserEntityByUsernameAsync(UserRegistration.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User creation failed.");
                return Page();
            }

            var claimsPrincipal = _authService.GetClaimsPrincipal(user);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToPage("/Index");
        }

    }
}
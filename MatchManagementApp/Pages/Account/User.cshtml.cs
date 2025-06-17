using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public UserViewModel Profile { get; set; } = new();

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetProfileImageAsync();

            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");

            }
            var userDto = await _userService.GetUserByIdAsync(userId.Value);
            if (userDto == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Profile = UserMapper.ToUserViewModel(userDto);

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Account/Login");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId != null)
            {
                await _userService.DeleteUserAsync(userId.Value);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToPage("/Account/Register");
        }

        public async Task<IActionResult> OnPostChangeImageAsync()
        {
            if (ProfileImage == null || ProfileImage.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select an image file.");
                await LoadProfileAsync();
                await SetProfileImageAsync();
                return Page();
            }

            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await ProfileImage.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }
            await _userService.UpdateUserImageAsync(userId.Value, imageData);

            return RedirectToPage();
        }

        private async Task LoadProfileAsync()
        {
            var userId = await _userService.GetCurrentUserIdAsync(User);
            if (userId != null)
            {
                var userDto = await _userService.GetUserByIdAsync(userId.Value);
                if (userDto != null)

                {
                    Profile = UserMapper.ToUserViewModel(userDto);
                }
            }
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

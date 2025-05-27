using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public LoginModel(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var userEntity = await _userService.GetUserEntityByUsernameAsync(Input.Username);

        if (userEntity == null || !_userService.VerifyPassword(userEntity.PasswordHash, Input.Password))
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }

        var userDto = new UserCreateDto
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            Email = userEntity.Email,
            PasswordHash = userEntity.PasswordHash,
            Age = userEntity.Age,
            Gender = userEntity.Gender
        };

        var claimsPrincipal = _authService.GetClaimsPrincipal(userDto);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

        return RedirectToPage("/Index");
    }
}

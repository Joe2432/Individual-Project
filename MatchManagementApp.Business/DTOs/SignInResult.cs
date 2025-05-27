using System.Security.Claims;

public class SignInResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }
}
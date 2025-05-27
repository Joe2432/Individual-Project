using System.Security.Claims;

public class RegisterResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }
}

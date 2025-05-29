using System.Security.Claims;
public class AuthResultDto
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }
}

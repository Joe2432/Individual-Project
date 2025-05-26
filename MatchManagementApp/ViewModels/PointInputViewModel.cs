using System.ComponentModel.DataAnnotations;

public class PointInputViewModel
{
    [Required]
    public int MatchId { get; set; }

    [Required]
    public string PointType { get; set; } = string.Empty;

    [Required]
    public bool IsUserWinner { get; set; }

    public int NumberOfShots { get; set; } = 0;
}
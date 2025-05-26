using System.ComponentModel.DataAnnotations;

public class MatchCreationViewModel
{
    [Required]
    public string MatchType { get; set; } = "Singles";

    public string? PartnerName { get; set; }

    [Required]
    public string FirstOpponentName { get; set; } = string.Empty;

    public string? SecondOpponentName { get; set; }

    [Required]
    [Range(3, 5)]
    public int NrSets { get; set; } = 3;

    [Required]
    public string FinalSetType { get; set; } = "Normal Set";

    [Required]
    public string GameFormat { get; set; } = "With Advantage";

    [Required]
    public string Surface { get; set; } = "Hard";
}
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record GameWagerViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string CompanyKey { get; init; } = default!;
    [Required]
    public string Agent { get; init; } = default!;
    [Required]
    public DateTimeOffset StartTime { get; init; }
    [Required]
    public DateTimeOffset EndTime { get; init; }
}

using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record GameUrlByVirtualSportsViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string CompanyKey { get; init; } = default!;
    [Required]
    public string Agent { get; init; } = default!;
    [Required]
    public string PlayerCode { get; init; } = default!;
    [Required]
    public string Lang { get; init; } = default!;
}

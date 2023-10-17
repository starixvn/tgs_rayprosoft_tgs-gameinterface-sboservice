using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record FundViewModel
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
    public string OrderId { get; init; } = default!;
    [Required]
    [Range(minimum: 0, maximum: double.MaxValue)]
    public decimal Amount { get; init; }
}

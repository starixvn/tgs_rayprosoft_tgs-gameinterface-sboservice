using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record FundStatusViewModel
{
	[Required]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	public string CompanyKey { get; init; } = default!;
	[Required]
	public string OrderId { get; init; } = default!;
}

using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record AgentPresetBetSettingsViewModel
{
	[Required]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	public string CompanyKey { get; init; } = default!;
	[Required]
	public string Agent { get; init; } = default!;
	[Required]
	public string Currency { get; init; } = default!;
	[Required]
	[Range(minimum: 1, maximum: int.MaxValue)]
	public int Min { get; init; }
	[Required]
	[Range(minimum: 1, maximum: int.MaxValue)]
	public int Max { get; init; }
	[Required]
	[Range(minimum: 1, maximum: int.MaxValue)]
	public int MaxPerMatch { get; init; }
	[Required]
	[Range(minimum: 1, maximum: int.MaxValue)]
	public int CasinoTableLimit { get; init; }
}

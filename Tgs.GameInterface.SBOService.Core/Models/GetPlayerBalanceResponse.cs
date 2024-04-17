namespace Tgs.GameInterface.SBOService.Core.Models;

public record GetPlayerBalanceResponse
{
	public bool IsPlayerExist { get; init; }
	public decimal Balance { get; init; }
	public decimal Outstanding { get; init; }
}

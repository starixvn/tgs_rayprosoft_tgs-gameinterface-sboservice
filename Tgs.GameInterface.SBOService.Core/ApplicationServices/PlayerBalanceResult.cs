namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record PlayerBalanceResult
{
    public bool IsPlayerExist { get; init; }
    public decimal Balance { get; init; }
    public decimal FreeBalance => Balance - OutstandingBalance;
    public decimal OutstandingBalance { get; init; }
}

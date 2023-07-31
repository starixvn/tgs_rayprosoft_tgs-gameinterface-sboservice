namespace Tgs.GameInterface.SBOService.ViewModels;

public record PlayerBalanceResultViewModel(
    bool IsPlayerExist,
    decimal Balance,
    decimal FreeBalance,
    decimal OutstandingBalance);

namespace Tgs.GameInterface.SBOService.Core.Models;

public record FundResult(
    string TxnId,
    string Refno,
    decimal Balance);

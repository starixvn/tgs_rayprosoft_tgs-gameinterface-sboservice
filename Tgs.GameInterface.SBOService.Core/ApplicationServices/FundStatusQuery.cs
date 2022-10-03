namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string CompanyKey,
    string TxnId);

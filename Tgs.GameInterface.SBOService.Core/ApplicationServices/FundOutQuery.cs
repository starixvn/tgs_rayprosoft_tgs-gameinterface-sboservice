namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    string PlayerCode,
    string OrderId,
    decimal Amount);

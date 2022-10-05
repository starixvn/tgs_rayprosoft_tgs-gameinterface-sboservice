namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    string Username,
    decimal Amount);

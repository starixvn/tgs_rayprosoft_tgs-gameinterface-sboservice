namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameUrlByVirtualSportsQuery(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    string PlayerCode,
    string Lang);

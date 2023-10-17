namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameWagerBySportsBookQuery(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);

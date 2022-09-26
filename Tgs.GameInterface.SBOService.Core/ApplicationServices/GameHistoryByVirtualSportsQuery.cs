namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameHistoryByVirtualSportsQuery(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    DateTime StartTime,
    DateTime EndTime);

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameHistoryBySportsBookQuery(
    Uri ApiUrl,
    string CompanyKey,
    string Agent,
    DateTime StartTime,
    DateTime EndTime);

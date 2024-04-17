namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameWagerByVirtualSportsQuery(
	Uri ApiUrl,
	string CompanyKey,
	string Agent,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);

namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameWagerBySeamlessGameQuery(
	Uri ApiUrl,
	string CompanyKey,
	string Agent,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);

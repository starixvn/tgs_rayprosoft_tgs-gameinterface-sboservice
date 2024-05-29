namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameRecordBySeamlessGameQuery(
	Uri ApiUrl,
	string CompanyKey,
	string Agent,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);

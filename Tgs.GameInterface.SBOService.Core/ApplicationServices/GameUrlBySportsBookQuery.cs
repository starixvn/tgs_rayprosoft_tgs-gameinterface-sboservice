namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameUrlBySportsBookQuery(
	Uri ApiUrl,
	string CompanyKey,
	string Agent,
	string PlayerCode,
	string Lang,
	bool IsMobile,
	string Oddstyle = "MY",
	string Theme = "SBO",
	string Oddsmode = "double");

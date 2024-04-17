namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string CompanyKey,
	string Agent,
	string PlayerCode);

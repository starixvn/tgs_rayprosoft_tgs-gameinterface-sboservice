namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record FundInQuery(
	Uri ApiUrl,
	string CompanyKey,
	string Agent,
	string PlayerCode,
	string OrderId,
	decimal Amount);

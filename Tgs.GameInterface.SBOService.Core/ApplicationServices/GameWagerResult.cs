namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;

public record GameWagerResult(
	string PlayerCode,
	string ExternalWagerId,
	string ExternalGameId,
	string Status,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal PlayerWinLoss,
	decimal Turnover,
	decimal Settle,
	string AdditionalData);

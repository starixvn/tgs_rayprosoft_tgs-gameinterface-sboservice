namespace Tgs.GameInterface.SBOService.Core.ApplicationServices;
public record GameRecordBySeamlessGameResult(
	string PlayerCode,
	string ExternalRoundId,
	string ExternalWagerId,
	string ExternalGameId,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	string Status,
	decimal Bet,
	decimal Settle,
	decimal Turnover,
	decimal WinLoss,
	string AdditionalData);

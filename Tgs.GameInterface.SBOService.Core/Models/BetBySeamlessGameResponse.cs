namespace Tgs.GameInterface.SBOService.Core.Models;

public record BetBySeamlessGameResponse(
	string ExternalRoundId,
	string ExternalWagerId,
	string GameType,
	string PlayerCode,
	string Status,
	DateTimeOffset OrderTime,
	DateTimeOffset ModifyDate,
	DateTimeOffset SettleTime,
	DateTimeOffset WinLostDate,
	decimal Bet,
	decimal Turnover,
	decimal WinLoss,
	string AdditionalData);

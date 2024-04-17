using Tgs.GameInterface.SBOService.Protos;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record GameWagerResultViewModel(
	string PlayerCode,
	string ExternalWagerId,
	string ExternalGameId,
	GameHistoryStatus Status,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal PlayerWinLoss,
	decimal Turnover,
	decimal Settle,
	string AdditionalData);

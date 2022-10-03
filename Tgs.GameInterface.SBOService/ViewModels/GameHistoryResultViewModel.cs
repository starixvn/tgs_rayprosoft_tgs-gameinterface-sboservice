using Tgs.GameInterface.SBOService.Protos;

namespace Tgs.GameInterface.SBOService.ViewModels;

public record GameHistoryResultViewModel(
    string PlayerCode,
    string ExternalRoundId,
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

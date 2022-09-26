namespace Tgs.GameInterface.SBOService.Core.Models;

public record GameHistoryResult(
    string PlayerCode,
    string ExternalRoundId,
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

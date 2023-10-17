namespace Tgs.GameInterface.SBOService.Core.Models;

public record BetBySportsBookResponse(
    string ExternalWagerId,
    string PlayerCode,
    string SportsType,
    string Status,
    DateTimeOffset OrderTime,
    DateTimeOffset ModifyDate,
    DateTimeOffset SettleTime,
    DateTimeOffset WinLostDate,
    decimal Bet,
    decimal ValidBet,
    decimal Turnover,
    decimal PlayerWinLoss,
    string AdditionalData);

namespace Tgs.GameInterface.SBOService.Core.Models;

public record BetByVirtualSportsResponse(
    string ExternalWagerId,
    string PlayerCode,
    string ProductType,
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

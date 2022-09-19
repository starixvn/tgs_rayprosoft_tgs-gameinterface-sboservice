using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SBOService.Core.Models;

public record BetTransactionResponse
{
    [JsonPropertyName("refNo")]
    public string RefNo { get; init; } = default!;
    [JsonPropertyName("username")]
    public string Username { get; init; } = default!;
    [JsonPropertyName("sportsType")]
    public string SportsType { get; init; } = default!;
    [JsonPropertyName("orderTime")]
    public DateTimeOffset OrderTime { get; init; }
    [JsonPropertyName("winLostDate")]
    public DateTimeOffset? WinLostDate { get; init; }
    [JsonPropertyName("settleTime")]
    public DateTimeOffset? SettleTime { get; init; }
    [JsonPropertyName("modifyDate")]
    public DateTimeOffset ModifyDate { get; init; }
    [JsonPropertyName("odds")]
    public decimal Odds { get; init; }
    [JsonPropertyName("oddsStyle")]
    public string OddsStyle { get; init; } = default!;
    [JsonPropertyName("stake")]
    public decimal Stake { get; init; }
    [JsonPropertyName("actualStake")]
    public decimal ActualStake { get; init; }
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = default!;
    [JsonPropertyName("status")]
    public string Status { get; init; } = default!;
    [JsonPropertyName("winLost")]
    public decimal WinLost { get; init; }
    [JsonPropertyName("turnover")]
    public decimal Turnover { get; init; }
    [JsonPropertyName("turnoverByStake")]
    public decimal TurnoverByStake { get; init; }
    [JsonPropertyName("turnoverByActualStake")]
    public decimal TurnoverByActualStake { get; init; }
    [JsonPropertyName("netTurnoverByStake")]
    public decimal NetTurnoverByStake { get; init; }
    [JsonPropertyName("netTurnoverByActualStake")]
    public decimal NetTurnoverByActualStake { get; init; }
    [JsonPropertyName("isHalfWonLose")]
    public bool IsHalfWonLose { get; init; }
    [JsonPropertyName("isLive")]
    public bool IsLive { get; init; }
    [JsonPropertyName("maxWinWithoutActualStake")]
    public decimal MaxWinWithoutActualStake { get; init; }
    [JsonPropertyName("ip")]
    public string Ip { get; init; } = default!;
    [JsonPropertyName("subBet")]
    public IEnumerable<BetTransactionSubResponse> SubBet { get; init; } = default!;
    [JsonPropertyName("sportType ")]
    public string SportType { get; init; } = default!;
    [JsonPropertyName("isSystemTagRisky ")]
    public bool IsSystemTagRisky { get; init; } = default!;
    [JsonPropertyName("voidReason")]
    public string VoidReason { get; init; } = default!;
    [JsonPropertyName("isCustomerTagRisky ")]
    public bool IsCustomerTagRisky { get; init; } = default!;
    [JsonPropertyName("topDownline")]
    public string TopDownline { get; init; } = default!;
}

using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SBOService.Core.Models;

public record BetTransactionSubResponse
{
    [JsonPropertyName("betOption")]
    public string BetOption { get; init; } = default!;
    [JsonPropertyName("marketType")]
    public string MarketType { get; init; } = default!;
    [JsonPropertyName("hdp")]
    public float Hdp { get; init; } = default!;
    [JsonPropertyName("odds")]
    public decimal Odds { get; init; } = default!;
    [JsonPropertyName("league")]
    public string League { get; init; } = default!;
    [JsonPropertyName("match")]
    public string Match { get; init; } = default!;
    [JsonPropertyName("status")]
    public string Status { get; init; } = default!;
    [JsonPropertyName("winlostDate")]
    public DateTimeOffset? WinlostDate { get; init; }
    [JsonPropertyName("liveScore")]
    public string LiveScore { get; init; } = default!;
    [JsonPropertyName("htScore")]
    public string HtScore { get; init; } = default!;
    [JsonPropertyName("ftScore")]
    public string FtScore { get; set; } = default!;
    [JsonPropertyName("customeizedBetType")]
    public string CustomeizedBetType { get; set; } = default!;
    [JsonPropertyName("kickOffTime")]
    public DateTimeOffset KickOffTime { get; set; }
    [JsonPropertyName("isHalfWonLose")]
    public bool IsHalfWonLose { get; set; }
}

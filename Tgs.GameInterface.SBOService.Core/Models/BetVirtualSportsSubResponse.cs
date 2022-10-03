using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SBOService.Core.Models;

public record BetVirtualSportsSubResponse
{
    [JsonPropertyName("htScore")]
    public string HtScore { get; set; } = default!;
    [JsonPropertyName("ftScore")]
    public string FtScore { get; set; } = default!;
    [JsonPropertyName("betOption")]
    public string BetOption { get; set; } = default!;
    [JsonPropertyName("marketType")]
    public string MarketType { get; set; } = default!;
    [JsonPropertyName("hdp")]
    public string Hdp { get; set; } = default!;
    [JsonPropertyName("odds")]
    public decimal Odds { get; set; }
    [JsonPropertyName("match")]
    public string Match { get; set; } = default!;
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
    [JsonPropertyName("winLostDate")]
    public DateTimeOffset WinLostDate { get; set; }
}

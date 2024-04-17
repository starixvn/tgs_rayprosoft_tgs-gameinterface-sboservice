using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SBOService.Core.Models;

public class BetVirtualSportsResponse
{
	[JsonPropertyName("gameId")]
	public int GameId { get; set; }

	[JsonPropertyName("odds")]
	public decimal Odds { get; init; }

	[JsonPropertyName("oddsStyle")]
	public string OddsStyle { get; init; } = default!;

	[JsonPropertyName("actualStake")]
	public decimal ActualStake { get; init; }

	[JsonPropertyName("turnover")]
	public decimal Turnover { get; init; }

	[JsonPropertyName("productType")]
	public string ProductType { get; set; } = default!;

	[JsonPropertyName("subBet")]
	public IEnumerable<BetVirtualSportsSubResponse> SubBet { get; set; } = default!;

	[JsonPropertyName("orderTime")]
	public DateTimeOffset OrderTime { get; init; }

	[JsonPropertyName("modifyDate")]
	public DateTimeOffset ModifyDate { get; init; }

	[JsonPropertyName("settleTime")]
	public DateTimeOffset SettleTime { get; init; }

	[JsonPropertyName("winLostDate")]
	public DateTimeOffset WinLostDate { get; init; }

	[JsonPropertyName("refNo")]
	public string RefNo { get; init; } = default!;

	[JsonPropertyName("username")]
	public string Username { get; init; } = default!;

	[JsonPropertyName("currency")]
	public string Currency { get; init; } = default!;

	[JsonPropertyName("stake")]
	public decimal Stake { get; init; }

	[JsonPropertyName("winLost")]
	public decimal WinLost { get; init; }

	[JsonPropertyName("status")]
	public string Status { get; init; } = default!;

	[JsonPropertyName("topDownline")]
	public string TopDownline { get; init; } = default!;
}

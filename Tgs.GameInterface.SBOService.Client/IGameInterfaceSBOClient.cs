using Tgs.GameInterface.SBOService.Protos;

namespace Tgs.GameInterface.SBOService.Client;

public interface IGameInterfaceSBOClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask AgentPresetBetSettingsAsync(Uri apiUrl, string companyKey, string agent, string currency, int min, int max, int maxPerMatch, int casinoTableLimit, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string companyKey, string agent, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string companyKey, string agent, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, string companyKey, string orderId, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlBySportsBookAsync(Uri apiUrl, string companyKey, string agent, string playerCode, string lang, bool isMobile, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlByVirtualSportsAsync(Uri apiUrl, string companyKey, string agent, string playerCode, string lang, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string companyKey, string agent, string playerCode, CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(Uri apiUrl, string companyKey, string agent, string playerCode, CancellationToken cancellationToken = default);

	ValueTask PlayerKickAsync(Uri apiUrl, string companyKey, string agent, string playerCode, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerInfo> GameWagerBySportsBookAsync(Uri apiUrl, string companyKey, string agent, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerInfo> GameWagerByVirtualSportsAsync(Uri apiUrl, string companyKey, string agent, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);
}

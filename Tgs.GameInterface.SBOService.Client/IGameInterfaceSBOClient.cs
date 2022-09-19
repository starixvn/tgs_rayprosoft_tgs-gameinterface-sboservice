using Tgs.GameInterface.SBOService.Protos;

namespace Tgs.GameInterface.SBOService.Client;

public interface IGameInterfaceSBOClient
{
    ValueTask AgentPresetBetSettingsAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string currency,
        int min,
        int max,
        int maxPerMatch,
        int casinoTableLimit,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri ApiUrl,
        string CompanyKey,
        string Agent,
        string Username,
        decimal Amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri ApiUrl,
        string CompanyKey,
        string Agent,
        string Username,
        decimal Amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri ApiUrl,
        string CompanyKey,
        string TxnId,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GameUrlBySportsBookAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        string lang,
        bool isMobile,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GameUrlByVirtualSportsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        string lang,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask PlayerKickAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        CancellationToken cancellationToken = default);
}

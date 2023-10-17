using Tgs.GameInterface.SBOService.Core.Models;

namespace Tgs.GameInterface.SBOService.Core;

public interface IGameApiClient
{
    ValueTask RegisterAgentAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string currency,
        int min,
        int max,
        int maxPerMatch,
        int casinoTableLimit,
        CancellationToken cancellationToken = default);

    ValueTask<UpdateAgentPresetBetSettingsResponse> UpdateAgentPresetBetSettingsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        int min,
        int max,
        int maxPerMatch,
        int casinoTableLimit,
        CancellationToken cancellationToken = default);

    ValueTask RegisterPlayerAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string agent,
        CancellationToken cancellationToken = default);

    ValueTask<LoginResponse> LoginAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string portfolio,
        CancellationToken cancellationToken = default);

    ValueTask<TransactionResponse> DepositAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string txnId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<TransactionResponse> WithdrawAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string txnId,
        decimal amount,
        bool isFullAmount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> CheckTransactionStatusAsync(
        Uri apiUrl,
        string companyKey,
        string txnId,
        CancellationToken cancellationToken = default);

    ValueTask<GetPlayerBalanceResponse> GetPlayerBalanceAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask LogoutAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetBySportsBookResponse> GetBetListByModifyDateAgentBySportsBookAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetByVirtualSportsResponse> GetBetListByModifyDateAgentByVirtualSportsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);
}

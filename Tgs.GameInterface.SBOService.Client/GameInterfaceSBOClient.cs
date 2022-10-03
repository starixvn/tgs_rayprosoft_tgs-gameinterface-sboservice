using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.SBOService.Protos;

namespace Tgs.GameInterface.SBOService.Client;

internal class GameInterfaceSBOClient : IGameInterfaceSBOClient
{
    private readonly GameInterfaceSBOService.GameInterfaceSBOServiceClient _client;

    public GameInterfaceSBOClient(GameInterfaceSBOService.GameInterfaceSBOServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask AgentPresetBetSettingsAsync(
        Uri apiUrl,
        string companyKey,
        string username,
        string currency,
        int min,
        int max,
        int maxPerMatch,
        int casinoTableLimit,
        CancellationToken cancellationToken = default)
    {
        var request = new AgentPresetBetSettingsRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Username = username,
            Currency = currency,
            Min = min,
            Max = max,
            MaxPerMatch = maxPerMatch,
            CasinoTableLimit = casinoTableLimit
        };

        _ = await _client
                .AgentPresetBetSettingsAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username,
            Amount = amount
        };

        return await _client
                 .FundInAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username,
            Amount = amount
        };

        return await _client
                 .FundOutAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string companyKey,
        string txnId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            TxnId = txnId
        };

        var reply = await _client
                .FundStatusAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        return reply.IsOrderExist;
    }

    public async IAsyncEnumerable<GameHistoryInfo> GameHistoryBySportsBookAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        DateTime startTime,
        DateTime endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistoryRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
                 .GameHistoryBySportsBookAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<GameHistoryInfo> GameHistoryByVirtualSportsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        DateTime startTime,
        DateTime endTime,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistoryRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
                 .GameHistoryByVirtualSportsAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async ValueTask<GameUrlReply> GameUrlBySportsBookAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        string lang,
        bool isMobile,
        CancellationToken cancellationToken = default)
    {
        var request = new GameUrlBySportsBookRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username,
            Lang = lang,
            IsMobile = isMobile
        };

        return await _client
                 .GameUrlBySportsBookAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }

    public async ValueTask<GameUrlReply> GameUrlByVirtualSportsAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        string lang,
        CancellationToken cancellationToken = default)
    {
        var request = new GameUrlByVirtualSportsRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username,
            Lang = lang
        };

        return await _client
                 .GameUrlByVirtualSportsAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username
        };

        return await _client
                 .PlayerBalanceAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerCreateRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username
        };

        _ = await _client
                 .PlayerCreateAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }

    public async ValueTask PlayerKickAsync(
        Uri apiUrl,
        string companyKey,
        string agent,
        string username,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerKickRequest()
        {
            ApiUrl = apiUrl.ToString(),
            CompanyKey = companyKey,
            Agent = agent,
            Username = username
        };

        _ = await _client
                 .PlayerKickAsync(
                     request: request,
                     cancellationToken: cancellationToken)
                 .ConfigureAwait(false);
    }
}
